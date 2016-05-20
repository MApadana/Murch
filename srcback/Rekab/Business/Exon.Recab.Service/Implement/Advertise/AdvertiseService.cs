using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Domain.MongoDb;
using System;
using System.Linq;
using System.Collections.Generic;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Domain.Constant.Prodoct;
using System.Net;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Domain.Constant.Media;
using System.Web.Script.Serialization;
using MongoDB.Bson.Serialization;
using Exon.Recab.Service.Implement.PolicySystemConfig;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Model.TodayPriceModel;
using Exon.Recab.Service.Constant;
using Exon.Recab.Domain.Entity.AlertModule;
using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Service.Implement.Helper;
using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Service.Implement.Email;

namespace Exon.Recab.Service.Implement.Advertise
{
    public class AdvertiseService

    {
        #region init
        /// <summary>
        ///   ماتی سلکت ها فرزند ندارند
        ///   کاستوم ولیو ها فرزند ندارند
        ///   کاستوم ولیو ها در اگریگیشن نمی آیند
        /// </summary>

        private SdbContext _sdb;

        private MdbContext _mdb;

        private readonly ProductPolicyService _PolicyService;

        internal AdvertiseService(ref SdbContext sdb, ref MdbContext mdb)
        {
            _sdb = sdb;
            _mdb = mdb;
            _PolicyService = new ProductPolicyService(ref sdb);
        }

        public AdvertiseService()
        {
            _sdb = new SdbContext();
            _mdb = new MdbContext();
            _PolicyService = new ProductPolicyService(ref _sdb);
        }

        #endregion

        #region ADD
        public long AddNewProdoct(long userId,
                                  long categoryId,
                                  long? packageId,
                                  string tell,
                                  string description,
                                  long? exchangeCategoryId,
                                  List<ProductSelectItemModel> productItems,
                                  List<MediaModel> media,
                                  long? dealershipId,
                                  ref long count)
        {
            #region Validation

            if (!_PolicyService.ValidateDefaultPictureCount(media.Count))
                throw new RecabException((int)ExceptionType.PictureCountInvalid);

            int inputDistinctCount = productItems.Select(pi => pi.CategoryFeatureId).Distinct().Count();

            if (inputDistinctCount != productItems.Count)
                throw new RecabException((int)ExceptionType.SomeCategoryFeatureRedundant);


            #region Headr
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);


            Category category = _sdb.Categoris.Find(categoryId);


            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            if (exchangeCategoryId.HasValue)
            {
                Category excategory = _sdb.Categoris.Find(exchangeCategoryId.Value);

                if (excategory == null)
                    throw new RecabException((int)ExceptionType.ExchangeCategoryNotFound);
            }

            UserPackageCredit package = new UserPackageCredit();

            if (packageId.HasValue)
            {
                UserPackageCredit packageTemp = _sdb.UserPackageCredits.Find(packageId);

                if (packageTemp == null)
                    throw new RecabException((int)ExceptionType.UserPackageCreditNotFound);

                package = packageTemp;

                GC.Collect();
            }

            if (dealershipId.HasValue)
            {
                Dealership dealership = _sdb.Dealerships.Find(dealershipId.Value);

                if (dealership == null || dealership.UserId != user.Id)
                    throw new RecabException((int)ExceptionType.DealershipNotFound);
            }

            #endregion

            #region CategoryFeature
            List<CategoryFeature> ADSCategoryFeature = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId && cf.AvailableInADS).ToList();

            ///validate item  and concat to cf
            var ClientItems = ADSCategoryFeature.Join(productItems,
                                                      i => i.Id,
                                                      pi => pi.CategoryFeatureId,
                                                      (i, pi) => new
                                                      {
                                                          CategoryFeature = i,
                                                          ProductSelectItem = pi
                                                      }).OrderBy(c => c.CategoryFeature.OrderId).ToList();

            /// اگر تعداد آیتم های تایید شده با همه تعداد همه آیتم ها برابر نبود 
            if (ClientItems.Count != inputDistinctCount)
                throw new RecabException((int)ExceptionType.InvalidInsertCategoryFeature);

            #endregion


            #region FeatureValue

            List<ProductFeatureValue> ProductFeatureValues = new List<ProductFeatureValue>();

            List<long?> HideList = new List<long?>();

            foreach (var item in ClientItems)
            {
                #region  NO CUSTOM VALUE
                if (!item.CategoryFeature.HasCustomValue)
                {
                    ///اگر چیزی انتخاب نشده بود
                    if (item.ProductSelectItem.FeatureValueId.Count == 0)
                        throw new RecabException((int)ExceptionType.CategoryFeatureModelStateErrorNullId, item.CategoryFeature.Title);

                    ///اگر سینگل بود و چند موردانتخاب نشده بود
                    if (!item.CategoryFeature.HasMultiSelectValue && item.ProductSelectItem.FeatureValueId.Count > 1)
                        throw new RecabException((int)ExceptionType.CategoryFeatureModelStateErrorWrongMulti, item.CategoryFeature.Title);

                    ///  اگر سینگل بود  واشتباه بود
                    if (!item.CategoryFeature.HasMultiSelectValue &&
                        item.ProductSelectItem.FeatureValueId.Count == 1 &&
                        !item.CategoryFeature.FeatureValueList.Any(fv => fv.Id == item.ProductSelectItem.FeatureValueId.First()))

                        throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorWrongId, item.CategoryFeature.Title);


                    if (item.CategoryFeature.HasMultiSelectValue)
                    {
                        foreach (var temp in item.ProductSelectItem.FeatureValueId)
                        {
                            /// اگر مالتی آیدی اشتباه انتخاب کرده بود
                            if (!item.CategoryFeature.FeatureValueList.Any(fv => fv.Id == temp))
                                throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorWrongId, item.CategoryFeature.Title);
                        }
                    }


                }
                #endregion

                #region   CUSTOM VALUE
                else
                {
                    Regex regex = new Regex(item.CategoryFeature.Pattern, RegexOptions.IgnoreCase);

                    Match match = regex.Match(item.ProductSelectItem.CustomValue ?? "");

                    ///فرمت دیتای وارد شده برابر نبود
                    if (!match.Success)
                        throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorRegex, item.CategoryFeature.Title);

                }
                #endregion

                #region DEPENDENCI

                ///بدون سرشاخه
                if (item.CategoryFeature.ParentList.Where(cf => cf.CategoryFeature.AvailableInADS).Count() == 0)
                {
                    ProductFeatureValues.Add(new ProductFeatureValue
                    {
                        CategoryFeatureId = item.CategoryFeature.Id,
                        CustomValue = item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.CustomValue : "",
                        ListFeatureValue = !item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.FeatureValueId.Select(fvIds => new ProductFeatureValueFeatureValueItem
                        {
                            FeatureValueId = fvIds
                        }).ToList() : new List<ProductFeatureValueFeatureValueItem>()
                    });

                    foreach (var fvitem in item.ProductSelectItem.FeatureValueId)
                    {

                        HideList = HideList.Concat(item.CategoryFeature.FeatureValueList.FirstOrDefault(fv => fv.Id == fvitem).HideList.Select(h => h.CategoryFeatureHideId).ToList()).ToList();
                    }

                }

                ///با سرشاخه
                if (item.CategoryFeature.ParentList.Where(cf => cf.CategoryFeature.AvailableInADS).Count() > 0)
                {
                    foreach (var par in item.CategoryFeature.ParentList.Where(cf => cf.CategoryFeature.AvailableInADS))
                    {
                        ProductFeatureValue parentItem = ProductFeatureValues.FirstOrDefault(pfv => pfv.CategoryFeatureId == par.CategoryFeatureId);

                        if (parentItem == null && parentItem.ListFeatureValue.Count != 1)
                            throw new RecabException((int)ExceptionType.ParentCategoryNotFound);

                        FeatureValue parentFeatureValue = ADSCategoryFeature.FirstOrDefault(cf => cf.Id == par.CategoryFeatureId)
                                                                            .FeatureValueList.FirstOrDefault(fv => fv.Id == parentItem.ListFeatureValue.FirstOrDefault().FeatureValueId);


                        foreach (var n in item.ProductSelectItem.FeatureValueId)
                        {
                            if (!parentFeatureValue.ChildList.Any(child => child.FeatureValueId == n))
                                throw new RecabException();
                        }

                    }

                    ProductFeatureValues.Add(new ProductFeatureValue
                    {
                        CategoryFeatureId = item.CategoryFeature.Id,
                        CustomValue = item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.CustomValue : "",
                        ListFeatureValue = item.ProductSelectItem.FeatureValueId.Select(fvIds => new ProductFeatureValueFeatureValueItem
                        {
                            FeatureValueId = fvIds
                        }).ToList()
                    });

                    foreach (var fvitem in item.ProductSelectItem.FeatureValueId)
                    {

                        HideList = HideList.Concat(item.CategoryFeature.FeatureValueList.FirstOrDefault(fv => fv.Id == fvitem).HideList.Select(h => h.CategoryFeatureHideId).ToList()).ToList();
                    }

                }
                #endregion

            }

            #region REQIRED
            List<CategoryFeature> RequiredCategoryFeature = ADSCategoryFeature.Where(cf => cf.RequiredInADInsert && !HideList.Any(h => h.Value == cf.Id)).ToList();

            if (RequiredCategoryFeature.Count != productItems.Join(RequiredCategoryFeature, p => p.CategoryFeatureId, r => r.Id, (p, r) => p).Count())
                throw new RecabException((int)ExceptionType.MissedInsertCategoryFeature);
            #endregion

            #endregion

            #endregion

            #region Add prodoct Header

            Product newProduct = new Product
            {
                CategoryId = categoryId,
                Status = ProdoctStatus.غیرفعال,
                InsertDate = DateTime.UtcNow,
                WebVisitCount = 0,
                AndroidVisitCount = 0,
                IosVisitCount = 0,
                DealershipId = dealershipId,
                UserPackageCreditId = packageId,
                Description = description ?? " ",
                UserId = user.Id,
                Tell = tell ?? "",
                Priority = 0,
                UpdateCount = 0,
                AdminComment = "",
                CategoryExchangeId = exchangeCategoryId

            };

            if (packageId.HasValue)
            {
                _PolicyService.EnforcementInsert(ref newProduct, ref package);
            }
            else
            {
                _PolicyService.EnforcementInsert(ref newProduct);
            }

            _sdb.Product.Add(newProduct);


            _sdb.SaveChanges();

            if (media.Count > 0)
            {
                foreach (var item in media)
                {
                    _sdb.Media.Add(new Media
                    {
                        MediaURL = item.Url,
                        EntityType = EntityType.Product,
                        EntityId = newProduct.Id,
                        MediaType = (MediaType)item.Type,
                        Order = item.OrderId

                    });
                }

                _sdb.SaveChanges();
            }

            #endregion

            foreach (var item in ProductFeatureValues)
            {
                item.ProductId = newProduct.Id;

                newProduct.ProductFeatures.Add(item);
            }

            _sdb.SaveChanges();

            EmailService emailservice = new EmailService(ref _sdb, ref _mdb);

            emailservice.SendAdsInfo(newProduct);

            if (!newProduct.CategoryExchangeId.HasValue)
            {

                if (packageId.HasValue)
                {
                    MongoProdoctSave(newProduct);

                    Implement.Alert.AlertService _alertService = new Alert.AlertService(ref _sdb, ref _mdb);
                    count = _alertService.CountAlertForNewProduct(newProduct.Id);
                }
                else
                {
                    newProduct.Status = ProdoctStatus.فعال;

                    MongoProdoctSave(newProduct);

                    Implement.Alert.AlertService _alertService = new Alert.AlertService(ref _sdb, ref _mdb);
                    count = _alertService.CountAlertForNewProduct(newProduct.Id);

                    newProduct.Status = ProdoctStatus.انتظاربرای_تایید;

                    this.MongoProductUpdate(newProduct);

                }

            }


            return newProduct.Id;
        }


        public long AddNewProdoctWithExchange(long userId,
                                              long categoryId,
                                              string description,
                                              long? packageId,
                                              string tell,
                                              List<ProductSelectItemModel> productItems,
                                              List<MediaModel> media,
                                              long exchangeCategoryId,
                                              List<ProductSelectItemModel> exchangeItems,
                                              long? dealershipId,
                                              ref long count
                                                )
        {

            long prodouctId = 0;


            prodouctId = this.AddNewProdoct(userId: userId,
                                categoryId: categoryId,
                                description: description,
                                packageId: packageId,
                                tell: tell,
                                exchangeCategoryId: exchangeCategoryId,
                                productItems: productItems,
                                media: media,
                                dealershipId: dealershipId,
                                count: ref count
                                );

            if (prodouctId > 0)
            {
                Product prodouct = _sdb.Product.Find(prodouctId);

                Category exchangeCategory = _sdb.Categoris.Find(exchangeCategoryId);

                if (exchangeCategory == null)
                    throw new RecabException("آگهی ثبت شد ولی گروه معاوضه یافت نشد", HttpStatusCode.OK);

                prodouct.CategoryExchangeId = exchangeCategory.Id;


                if (exchangeItems.Count > 0)
                {
                    List<CategoryFeature> InsertList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == exchangeCategoryId && cf.AvailableInADS && cf.AvailableInExchange).ToList();

                    var inputItems = InsertList.Join(exchangeItems, i => i.Id, pi => pi.CategoryFeatureId, (i, pi) => new { ExCategoryFeature = i, ExchangeSelectItem = pi }).ToList();


                    foreach (var item in inputItems)
                    {
                        ExchangeFeatureValue exchangeFeatureValue = new ExchangeFeatureValue();

                        exchangeFeatureValue.CategoryFeatureId = item.ExCategoryFeature.Id;

                        if (item.ExCategoryFeature.HasCustomValue)//no custom value
                        {
                            exchangeFeatureValue.CustomValue = item.ExchangeSelectItem.CustomValue;
                        }
                        else
                        {
                            if (item.ExCategoryFeature.FeatureValueList.Any(ecf => ecf.Id == item.ExchangeSelectItem.FeatureValueId.First()))
                            {
                                exchangeFeatureValue.FeatureValueId = item.ExchangeSelectItem.FeatureValueId.First();

                            }

                        }

                        prodouct.ExchangeFeatureValues.Add(exchangeFeatureValue);
                    }

                }

                _sdb.SaveChanges();



                if (packageId.HasValue)
                {
                    MongoProdoctSave(prodouct);

                    Implement.Alert.AlertService _alertService = new Alert.AlertService(ref _sdb, ref _mdb);
                    count = _alertService.CountAlertForNewProduct(prodouct.Id);
                }
                else
                {
                    prodouct.Status = ProdoctStatus.فعال;

                    MongoProdoctSave(prodouct);

                    Implement.Alert.AlertService _alertService = new Alert.AlertService(ref _sdb, ref _mdb);
                    count = _alertService.CountAlertForNewProduct(prodouct.Id);

                    prodouct.Status = ProdoctStatus.غیرفعال;

                    this.MongoProductUpdate(prodouct);

                }

                return prodouct.Id;

            }

            throw new RecabException((int)ExceptionType.InternalError);
        }

        #endregion

        #region Edit

        public long EditProdoct(long productId,
                                long userId,
                                string tell,
                                string description,
                                List<ProductSelectItemModel> productItems,
                                List<MediaModel> media,
                                long? dealershipId,
                                long? exchangeCategoryId,
                                bool isExchange)
        {
            #region Validation

            #region BaseCheck

            if (exchangeCategoryId.HasValue)
            {
                Category exchange = _sdb.Categoris.Find(exchangeCategoryId);
                if (exchange == null)
                    throw new RecabException((int)ExceptionType.CategoryNotFound);
            }
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            //if (!_PolicyService.ValidateDefaultPictureCount(media.Count))
            //    throw new RecabException((int)ExceptionType.PictureCountInvalid);

            Product product = _sdb.Product.FirstOrDefault(p => p.UserId == userId && p.Id == productId);

            if (product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            if (product.UpdateCount == 0)
                throw new RecabException((int)ExceptionType.UpdateCountLimit);


            int inputDistinctCount = productItems.Select(pi => pi.CategoryFeatureId).Distinct().Count();


            if (inputDistinctCount != productItems.Count)
                throw new RecabException((int)ExceptionType.SomeCategoryFeatureRedundant);


            if (!product.DealershipId.HasValue && dealershipId.HasValue)
                throw new RecabException((int)ExceptionType.DealershipCantAdd);

            if (product.DealershipId.HasValue && !dealershipId.HasValue)
                throw new RecabException((int)ExceptionType.DealershipCantRemove);


            if (product.DealershipId.HasValue && dealershipId.HasValue)
            {
                Dealership dealership = _sdb.Dealerships.Find(dealershipId.Value);

                if (dealership == null || dealership.UserId != product.UserId)
                    throw new RecabException((int)ExceptionType.DealershipNotFound);
            }

            #endregion

            #region ResetData

            _sdb.Media.RemoveRange(_sdb.Media.Where(m => m.EntityType == EntityType.Product && m.EntityId == product.Id));

            #endregion

            #region CategoryFeature
            List<CategoryFeature> ADSCategoryFeature = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == product.CategoryId && cf.AvailableInADS).ToList();

            ///validate item  and concat to cf
            var ClientItems = ADSCategoryFeature.Join(productItems,
                                                      i => i.Id,
                                                      pi => pi.CategoryFeatureId,
                                                      (i, pi) => new
                                                      {
                                                          CategoryFeature = i,
                                                          ProductSelectItem = pi
                                                      }).OrderBy(c => c.CategoryFeature.OrderId).ToList();

            /// اگر تعداد آیتم های تایید شده با همه تعداد همه آیتم ها برابر نبود 
            if (ClientItems.Count != inputDistinctCount)
                throw new RecabException((int)ExceptionType.InvalidInsertCategoryFeature);

            #endregion


            #region FeatureValue

            List<ProductFeatureValue> ProductFeatureValues = new List<ProductFeatureValue>();

            List<long?> HideList = new List<long?>();


            foreach (var item in ClientItems)
            {
                #region  NO CUSTOM VALUE
                if (!item.CategoryFeature.HasCustomValue)
                {
                    ///اگر چیزی انتخاب نشده بود
                    if (item.ProductSelectItem.FeatureValueId.Count == 0)
                        throw new RecabException((int)ExceptionType.CategoryFeatureModelStateErrorNullId, item.CategoryFeature.Title);

                    ///اگر سینگل بود و چند موردانتخاب نشده بود
                    if (!item.CategoryFeature.HasMultiSelectValue && item.ProductSelectItem.FeatureValueId.Count > 1)
                        throw new RecabException((int)ExceptionType.CategoryFeatureModelStateErrorWrongMulti, item.CategoryFeature.Title);

                    ///  اگر سینگل بود  واشتباه بود
                    if (!item.CategoryFeature.HasMultiSelectValue &&
                        item.ProductSelectItem.FeatureValueId.Count == 1 &&
                        !item.CategoryFeature.FeatureValueList.Any(fv => fv.Id == item.ProductSelectItem.FeatureValueId.First()))

                        throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorWrongId, item.CategoryFeature.Title);


                    if (item.CategoryFeature.HasMultiSelectValue)
                    {
                        foreach (var temp in item.ProductSelectItem.FeatureValueId)
                        {
                            /// اگر مالتی آیدی اشتباه انتخاب کرده بود
                            if (!item.CategoryFeature.FeatureValueList.Any(fv => fv.Id == temp))
                                throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorWrongId, item.CategoryFeature.Title);
                        }
                    }


                }
                #endregion

                #region   CUSTOM VALUE
                else
                {
                    Regex regex = new Regex(item.CategoryFeature.Pattern, RegexOptions.IgnoreCase);

                    Match match = regex.Match(item.ProductSelectItem.CustomValue ?? "");

                    ///فرمت دیتای وارد شده برابر نبود
                    if (!match.Success)
                        throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorRegex, item.CategoryFeature.Title);

                }
                #endregion

                #region DEPENDENCI

                ///بدون سرشاخه
                if (item.CategoryFeature.ParentList.Where(cf => cf.CategoryFeature.AvailableInADS).Count() == 0)
                {
                    ProductFeatureValues.Add(new ProductFeatureValue
                    {
                        CategoryFeatureId = item.CategoryFeature.Id,
                        CustomValue = item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.CustomValue : "",
                        ListFeatureValue = !item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.FeatureValueId.Select(fvIds => new ProductFeatureValueFeatureValueItem
                        {
                            FeatureValueId = fvIds
                        }).ToList() : new List<ProductFeatureValueFeatureValueItem>()
                    });

                    foreach (var fvitem in item.ProductSelectItem.FeatureValueId)
                    {

                        HideList = HideList.Concat(item.CategoryFeature.FeatureValueList.FirstOrDefault(fv => fv.Id == fvitem).HideList.Select(h => h.CategoryFeatureHideId).ToList()).ToList();
                    }

                }

                ///با سرشاخه
                if (item.CategoryFeature.ParentList.Where(cf => cf.CategoryFeature.AvailableInADS).Count() != 0)
                {
                    foreach (var par in item.CategoryFeature.ParentList.Where(cf => cf.CategoryFeature.AvailableInADS))
                    {
                        ProductFeatureValue parentItem = ProductFeatureValues.FirstOrDefault(pfv => pfv.CategoryFeatureId == par.CategoryFeatureId);

                        if (parentItem == null && parentItem.ListFeatureValue.Count != 1)
                            throw new RecabException((int)ExceptionType.ParentCategoryNotFound);

                        FeatureValue parentFeatureValue = ADSCategoryFeature.FirstOrDefault(cf => cf.Id == par.CategoryFeatureId)
                                                                            .FeatureValueList.FirstOrDefault(fv => fv.Id == parentItem.ListFeatureValue.FirstOrDefault().FeatureValueId);


                        foreach (var n in item.ProductSelectItem.FeatureValueId)
                        {
                            if (!parentFeatureValue.ChildList.Any(child => child.FeatureValueId == n))
                                throw new RecabException();
                        }

                    }

                    ProductFeatureValues.Add(new ProductFeatureValue
                    {
                        CategoryFeatureId = item.CategoryFeature.Id,
                        CustomValue = item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.CustomValue : "",
                        ListFeatureValue = item.ProductSelectItem.FeatureValueId.Select(fvIds => new ProductFeatureValueFeatureValueItem
                        {
                            FeatureValueId = fvIds
                        }).ToList()
                    });

                    foreach (var fvitem in item.ProductSelectItem.FeatureValueId)
                    {

                        HideList = HideList.Concat(item.CategoryFeature.FeatureValueList.FirstOrDefault(fv => fv.Id == fvitem).HideList.Select(h => h.CategoryFeatureHideId).ToList()).ToList();
                    }

                }
                #endregion

            }

            #region REQIRED
            List<CategoryFeature> RequiredCategoryFeature = ADSCategoryFeature.Where(cf => cf.RequiredInADInsert && !HideList.Any(h => h.Value == cf.Id)).ToList();

            if (RequiredCategoryFeature.Count != productItems.Join(RequiredCategoryFeature, p => p.CategoryFeatureId, r => r.Id, (p, r) => p).Count())
                throw new RecabException((int)ExceptionType.MissedInsertCategoryFeature);
            #endregion

            #endregion

            #endregion

            #region Add prodoct Header



            product.Status = ProdoctStatus.غیرفعال;
            product.DealershipId = dealershipId;
            product.Description = description ?? " ";
            product.Tell = tell ?? "";
            product.UpdateCount = product.UpdateCount - 1;
            product.CategoryExchangeId = exchangeCategoryId;

            _PolicyService.EnforcementUpdate(ref product);

            if (media.Count > 0)
            {
                foreach (var item in media)
                {
                    _sdb.Media.Add(new Media
                    {
                        MediaURL = item.Url,
                        EntityType = EntityType.Product,
                        EntityId = product.Id,
                        MediaType = (MediaType)item.Type,
                        Order = item.OrderId

                    });
                }


            }

            #endregion

            foreach (var item in product.ProductFeatures)
            {
                _sdb.ProductFeatureValueFeatureValueItems.RemoveRange(item.ListFeatureValue);
            }

            _sdb.ProductFeatureValues.RemoveRange(product.ProductFeatures);
            _sdb.SaveChanges();

            product.ProductFeatures = new List<ProductFeatureValue>();
            foreach (var item in ProductFeatureValues)
            {
                item.ProductId = product.Id;

                product.ProductFeatures.Add(item);
            }

            _sdb.SaveChanges();

            if (!isExchange)
                this.MongoProductUpdate(product);

            return product.Id;

        }



        public bool EditProdoctWithExchange(long productId,
                                            long userId,
                                            string tell,
                                            string description,
                                            List<ProductSelectItemModel> productItems,
                                            List<MediaModel> media,
                                            long exchangeCategoryId,
                                            List<ProductSelectItemModel> exchangeItems,
                                            long? dealershipId)
        {

            long id = this.EditProdoct(productId: productId,
                                       userId: userId,
                                       tell: tell,
                                       description: description,
                                       productItems: productItems,
                                       media: media,
                                       dealershipId: dealershipId,
                                       exchangeCategoryId: new long?(),
                                       isExchange: true);
            if (id == 0)
                throw new RecabException();

            Product product = _sdb.Product.Find(productId);

            Category exchangeCategory = _sdb.Categoris.Find(exchangeCategoryId);

            if (exchangeCategory == null)
                throw new RecabException("آگهی ثبت شد ولی گروه معاوضه یافت نشد", HttpStatusCode.OK);

            product.CategoryExchangeId = exchangeCategory.Id;

            _sdb.ExchangeFeatureValues.RemoveRange(product.ExchangeFeatureValues);

            if (exchangeItems.Count > 0)
            {
                List<CategoryFeature> EXInsertList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == exchangeCategoryId && cf.AvailableInADS && cf.AvailableInExchange).ToList();

                var EXinputItems = EXInsertList.Join(exchangeItems, i => i.Id, pi => pi.CategoryFeatureId, (i, pi) => new { ExCategoryFeature = i, ExchangeSelectItem = pi }).ToList();


                foreach (var item in EXinputItems)
                {
                    ExchangeFeatureValue exchangeFeatureValue = new ExchangeFeatureValue();

                    exchangeFeatureValue.CategoryFeatureId = item.ExCategoryFeature.Id;

                    if (item.ExCategoryFeature.HasCustomValue)//no custom value
                    {
                        exchangeFeatureValue.CustomValue = item.ExchangeSelectItem.CustomValue;
                    }
                    else
                    {
                        if (item.ExCategoryFeature.FeatureValueList.Any(ecf => ecf.Id == item.ExchangeSelectItem.FeatureValueId.First()))
                        {
                            exchangeFeatureValue.FeatureValueId = item.ExchangeSelectItem.FeatureValueId.First();

                        }

                    }

                    product.ExchangeFeatureValues.Add(exchangeFeatureValue);
                }

            }


            _sdb.SaveChanges();

            this.MongoProductUpdate(product);
            return true;

        }


        public bool RaiseUp(long productId)
        {

            return ProductHelperService.RaiseUp(productId, ref _mdb, ref _sdb);

        }

        #endregion

        #region Search
        public List<SearchResultItemViewModel> SearchProduct(string keyword,
                                                             long categoryId,
                                                             List<CFProdoctFilterModel> filters,
                                                             int type,
                                                             int sort,
                                                             bool packageReletive,
                                                             ref long Count,
                                                             long userId = 0,
                                                             int size = 1,
                                                             int page = 0)
        {

            return ProductHelperService.SearchProduct(keyword,
                                                      categoryId,
                                                      filters,
                                                      type,
                                                      sort,
                                                      packageReletive,
                                                      ref Count,
                                                      ref _mdb,
                                                      ref _sdb,
                                                      userId = 0,
                                                      size = 1,
                                                      page = 0);

        }


        public AggregateResultModel AggregateSearchProduct(string keyword,
                                                           long categoryId,
                                                           List<CFProdoctFilterModel> filters)
        {

            return ProductHelperService.AggregateSearchProduct(keyword,
                                                                categoryId,
                                                                filters,
                                                                ref _mdb,
                                                                ref _sdb);

        }



        public List<AdvertiseLocationSerarchViewModel> LocationSearchProduct(long userId,
                                                                            string keyword,
                                                                            long categoryId,
                                                                            double lat,
                                                                            double lng,
                                                                            int distance,
                                                                            List<CFProdoctFilterModel> filters,
                                                                            ref long count,
                                                                            int size = 1,
                                                                            int page = 0)
        {

            return ProductHelperService.LocationSearchProduct(userId,
                                                              keyword,
                                                              categoryId,
                                                              lat,
                                                              lng,
                                                              distance,
                                                              filters,
                                                              ref count,
                                                              ref _mdb ,
                                                              ref _sdb,
                                                              size = 1,
                                                              page = 0);
        }

        public ProcuctSearchDetailModel ProductDetailForSearch(long userId,
                                                               long productId,
                                                               bool isMobile)
        {
            Product product = _sdb.Product.Find(productId);

            if (product == null || product.Status != ProdoctStatus.فعال)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            long alertId = 0;
            AlertProduct alert = _sdb.AlertProduct.OrderByDescending(ap => ap.InsertDate).FirstOrDefault(ap => ap.Status == AlertProductStatus.فعال && userId == userId);
            if (alert != null)
                alertId = alert.Id;

            ProcuctSearchDetailModel model = new ProcuctSearchDetailModel();


            model.visit = ProductHelperService.UserProductVisitInAlert(userId, product.Id, alertId, ref _mdb);

            ProductHelperService.AddUserProductVisit(userId, product.Id, ref _mdb);

            if (isMobile)
            {
                product.AndroidVisitCount = product.AndroidVisitCount + 1;
            }
            else
            {
                product.WebVisitCount = product.WebVisitCount + 1;
            }

            ProductHelperService.UpdateProductVisit(product.Id, product.AndroidVisitCount + product.WebVisitCount, ref _mdb);
            _sdb.SaveChanges();



            model.confirmDate = product.ConfirmDate.HasValue ? product.ConfirmDate.Value.UTCToPrettyPersian() : "تایید نشده";

            model.email = product.User.Email;

            model.description = product.Description;

            model.categoryId = product.CategoryId;

            model.categoryTitle = product.Category.Title;

            model.tell = product.Tell ?? "";

            model.Id = product.Id;

            model.userName = product.User.FirstName + " " + product.User.LastName;

            model.hasDealership = product.DealershipId.HasValue;

            model.isFavorite = false;

            if (userId != 0)
            {

                model.isFavorite = _sdb.FavouriteProduct.Any(f => f.UserId == userId && f.ProductId == product.Id);
            }


            if (model.hasDealership)
                model.dealershipInfo = new ProcuctDealershipDetailModel
                {
                    title = product.Dealership.Title,
                    logoUrl = product.Dealership.LogoUrl,
                    dealershipId = product.DealershipId.Value,
                    address = product.Dealership.Address
                };

            if (_sdb.Media.Any(m => m.EntityType == EntityType.Product && m.EntityId == product.Id))
                model.mediaUrl = _sdb.Media.Where(m => m.EntityType == EntityType.Product && m.EntityId == product.Id).OrderBy(m => m.Order).Select(m => m.MediaURL).ToList();

            product.ProductFeatures = product.ProductFeatures.OrderBy(pf => pf.CategoryFeature.OrderId).ToList();


            var temp = product.ProductFeatures.Where(cf => cf.CategoryFeature.AvailableInTitle).OrderBy(cf => cf.CategoryFeature.TitleOrder);

            string ADTitle = "";

            foreach (var item in temp)
            {
                var titleItem = item.ListFeatureValue.FirstOrDefault();
                if (titleItem != null)
                {
                    ADTitle = ADTitle + " " + titleItem.FeatureValue.Title;
                }
                else
                {
                    ADTitle = ADTitle + item.CustomValue;
                }

            }
            model.title = ADTitle;

            model.advertiseCategoryFeatures = product.ProductFeatures.Where(pf => !pf.CategoryFeature.IsMap && !pf.CategoryFeature.IsRenge).Select(pcf => new ProductCategoryFeatureDetailViewModel
            {
                title = pcf.CategoryFeature.Title,
                categoryFeatureId = pcf.CategoryFeatureId,
                htmlType = pcf.CategoryFeature.Element.Title,
                featureValues = pcf.ListFeatureValue.Select(pfv => new ProductFeatureValueDetailViewModel
                {
                    featureValueId = pfv.FeatureValueId,
                    title = pfv.FeatureValue.Title
                }).ToList(),
                customValue = pcf.CustomValue != null ? (pcf.CustomValue.ToString() != "" ? pcf.CustomValue.ToString() : "") : ""


            }).ToList();

            #region reletive
            model.todayPriceSearchFilterItems = product.ProductFeatures.Where(pcf => pcf.CategoryFeature.AvailableTodayPrice && !pcf.CategoryFeature.HasCustomValue)
                                            .Select(pcf => new TodayPriceSearchFilterViewModel
                                            {
                                                categoryFeatureId = pcf.CategoryFeatureId,
                                                featureValueIds = pcf.ListFeatureValue.Select(pfv => pfv.FeatureValueId).ToList(),

                                            }).ToList();

            model.reviewSearchFilterItems = product.ProductFeatures.Where(pcf => pcf.CategoryFeature.AvailableInRVSearch && !pcf.CategoryFeature.HasCustomValue)
                               .Select(pcf => new TodayPriceSearchFilterViewModel
                               {
                                   categoryFeatureId = pcf.CategoryFeatureId,
                                   featureValueIds = pcf.ListFeatureValue.Select(pfv => pfv.FeatureValueId).ToList(),

                               }).ToList();


            model.hasReview = false;


            if (product.ProductFeatures.Where(pcf => pcf.CategoryFeature.AvailableInReview).Count() > 0)
            {
                var categoryFeatureReview = product.ProductFeatures.Where(pcf => pcf.CategoryFeature.AvailableInRVSearch)
                                                      .Select(pcf => new
                                                      {
                                                          CategoryFeature = pcf.CategoryFeature,
                                                          categoryfeatureId = pcf.CategoryFeatureId,

                                                          ListFeatureValue = pcf.ListFeatureValue.Select(pfv => new
                                                          {
                                                              FeatureValue = pfv.FeatureValue,
                                                              featureValueId = pfv.FeatureValueId,

                                                          }).ToList(),

                                                      }).ToList();

                var filter = categoryFeatureReview.Select(R => new TodayPriceFilterModelWithCategory
                {
                    CategoryFeature = R.CategoryFeature,
                    Filter = new TodayPriceFilterModel
                    {
                        CategoryFeatureId = R.categoryfeatureId,
                        FeatureValueId = R.ListFeatureValue.FirstOrDefault() == null ? 0 : R.ListFeatureValue.FirstOrDefault().featureValueId
                    }
                }).ToList();


                var match = ProductHelperService.GetMongoFilterSearch(categoryId: product.CategoryId, filter: filter, ads: false);

                JavaScriptSerializer scr = new JavaScriptSerializer();


                var projection = Builders<BsonDocument>.Projection.Exclude("_id");

                var MongoResult = _mdb.Reviews.Find(match).Project(projection).ToList();

                if (MongoResult.Count > 0)
                {
                    model.hasReview = true;
                }
            }
            model.articleSearchKeyword = "";
            var brand = product.ProductFeatures.Where(pcf => pcf.CategoryFeature.AvailableInArticle).OrderBy(pcf => pcf.CategoryFeature.OrderId).FirstOrDefault();

            BsonDocument Result = _mdb.Tag.Find(new BsonDocument { { "Title", brand.ListFeatureValue.FirstOrDefault().FeatureValue.Title } }).FirstOrDefault();

            if (Result.GetElement("ArticleItems").Value.AsBsonArray.Count > 0)
                model.articleSearchKeyword = brand.ListFeatureValue.FirstOrDefault().FeatureValue.Title;
            #endregion

            return model;

        }


        public ProcuctSearchDetailModel ProductDetailCompaire(long productId,
                                                              bool isMobile,
                                                              long categoryId)
        {

            Product product = _sdb.Product.Find(productId);

            if (product == null || product.Status != ProdoctStatus.فعال || product.CategoryId != categoryId)
                throw new RecabException((int)ExceptionType.CategoryNotCompatible);


            if (isMobile)
            {
                product.AndroidVisitCount = product.AndroidVisitCount + 1;
            }
            else
            {
                product.WebVisitCount = product.WebVisitCount + 1;
            }
            ProductHelperService.UpdateProductVisit(product.Id, product.AndroidVisitCount + product.WebVisitCount, ref _mdb);
            _sdb.SaveChanges();

            ProcuctSearchDetailModel model = new ProcuctSearchDetailModel();

            model.confirmDate = product.ConfirmDate.HasValue ? product.ConfirmDate.Value.UTCToPrettyPersian() : "تایید نشده";

            model.email = product.User.Email;

            model.description = product.Description;

            model.categoryId = product.CategoryId;

            model.categoryTitle = product.Category.Title;

            model.tell = product.Tell ?? "";

            model.Id = product.Id;

            model.userName = product.User.FirstName + " " + product.User.LastName;

            model.hasDealership = product.DealershipId.HasValue;

            string ADTitle = "";

            if (_sdb.Media.Any(m => m.EntityType == EntityType.Product && m.EntityId == product.Id))
                model.mediaUrl = _sdb.Media.Where(m => m.EntityType == EntityType.Product && m.EntityId == product.Id).OrderBy(m => m.Order).Select(m => m.MediaURL).ToList();

            product.ProductFeatures = product.ProductFeatures.OrderBy(pf => pf.CategoryFeature.OrderId).ToList();

            var temp = product.ProductFeatures.Where(cf => cf.CategoryFeature.AvailableInTitle).OrderBy(cf => cf.CategoryFeature.TitleOrder);


            foreach (var item in temp)
            {
                var titleItem = item.ListFeatureValue.FirstOrDefault();
                if (titleItem != null)
                {
                    ADTitle = ADTitle + " " + titleItem.FeatureValue.Title;
                }
                else
                {
                    ADTitle = ADTitle + item.CustomValue;
                }
            }

            model.advertiseCategoryFeatures = product.ProductFeatures.Where(cf => cf.CategoryFeature.AvailableInADCompaire).Select(pcf => new ProductCategoryFeatureDetailViewModel
            {
                title = pcf.CategoryFeature.Title,
                categoryFeatureId = pcf.CategoryFeatureId,
                featureValues = pcf.ListFeatureValue.Select(pfv => new ProductFeatureValueDetailViewModel
                {
                    featureValueId = pfv.FeatureValueId,
                    title = pfv.FeatureValue.Title
                }).ToList(),
                customValue = pcf.CustomValue != "" ? pcf.CustomValue.ToString() : ""


            }).ToList();

            List<CategoryFeature> compairCf = _sdb.CategoryFeatures.Where(cf => cf.AvailableInADCompaire).ToList();

            foreach (var item in compairCf)
            {

                if (!model.advertiseCategoryFeatures.Any(cf => cf.categoryFeatureId == item.Id))
                    model.advertiseCategoryFeatures.Add(new ProductCategoryFeatureDetailViewModel
                    {
                        title = item.Title,
                        categoryFeatureId = item.Id,
                        customValue = "",
                        featureValues = new List<ProductFeatureValueDetailViewModel>()
                    });
            }


            model.title = ADTitle;


            return model;
        }


        public List<SearchResultItemViewModel> GetAllFavouriteProduct(long userId,
                                                                      long categoryId,
                                                                      ref long count,
                                                                      int size = 1,
                                                                      int skip = 0)
        {

            return ProductHelperService.GetAllFavouriteProduct(userId,
                                                               categoryId,
                                                               ref count,
                                                               ref _mdb,
                                                               ref _sdb,
                                                               size = 1,
                                                               skip = 0);          


        }

        #endregion

        #region delete

        public bool delete(long productId)
        {
            Product product = _sdb.Product.Find(productId);

            if (product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);
            product.Status = ProdoctStatus.آرشیو;

            _sdb.SaveChanges();

            this.MongoProductUpdate(product);

            return true;

        }

        #endregion

        #region Mongo
        private void MongoProdoctSave(Product prodoct)
        {
            ProductHelperService.MongoProdoctSave(prodoct: prodoct, _mdb: ref _mdb, _sdb: ref _sdb);

        }

        internal void MongoProductUpdate(Product prodoct)
        {

            ProductHelperService.MongoProductUpdate(prodoct: prodoct, _mdb: ref _mdb, _sdb: ref _sdb);

        }

        #endregion


    }
}
