using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.AlertModule;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Constant;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.ProdoctModel.AlertModel;
using Exon.Recab.Service.Model.PublicModel;
using Exon.Recab.Service.Model.TodayPriceModel;
using Exon.Recab.Service.Implement.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Exon.Recab.Service.Implement.Alert
{
    public class AlertService
    {
        #region init

        private SdbContext _sdb;

        private MdbContext _mdb;

        public AlertService()
        {
            _sdb = new SdbContext();
            _mdb = new MdbContext();

        }

        internal AlertService(ref SdbContext sdb, ref MdbContext mdb)
        {
            this._sdb = sdb;
            this._mdb = mdb;
        }
        #endregion

        #region ADD      --newDon

        public bool AddNewAlert(long userId,
                                long categoryId,
                                string des,
                                bool sendEmail,
                                bool sendSMS,
                                bool sendPush,
                                List<SelectItemModel> alertItems)
        {
            #region Validation

            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            if (_sdb.AlertProduct.Any(ap => ap.UserId == user.Id))
                throw new RecabException((int)ExceptionType.UserNotFound);

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);


            List<CategoryFeature> InsertList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId && cf.AvailableADSAlert).ToList();


            string Title = "";
            List<SelectItemModel> ConfirmItems = alertItems.GetConfirmedItems(entityCategoryFeature: InsertList,
                                                                               reqiredCategoryFeature: InsertList.Where(cf => cf.RequiredInADSAlertInsert).ToList(),
                                                                               titleItems: InsertList.Where(cf => cf.AvailableInTitle).ToList(),
                                                                               title: ref Title);
            #endregion

            #region Header

            AlertProduct newAlertProduct = new AlertProduct
            {
                CategoryId = categoryId,
                Title = Title,
                UserId = user.Id,
                SendEmail = sendEmail,
                SendSMS = sendSMS,
                SendPush = sendPush,
                Status = AlertProductStatus.فعال,
                InsertDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddDays(5),
                Description = des

            };

            _sdb.AlertProduct.Add(newAlertProduct);

            _sdb.SaveChanges();


            #endregion

            #region FeatureValue

            foreach (var item in ConfirmItems)
            {
                FeatureValueAssign FeatureValueAssign;
                if (item.FeatureValueIds.Count > 0)
                {
                    FeatureValueAssign = new FeatureValueAssign
                    {
                        EntityType = EntityType.Alert,
                        CustomValue = "",
                        EntityId = newAlertProduct.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                    foreach (var fvItem in item.FeatureValueIds)
                    {
                        FeatureValueAssign.ListFeatureValue.Add(new FeatureValueAssignFeatureValueItem
                        {
                            FeatureValueId = fvItem
                        });
                    }

                }
                else
                {
                    FeatureValueAssign = new FeatureValueAssign
                    {
                        EntityType = EntityType.Alert,
                        CustomValue = item.CustomValue,
                        EntityId = newAlertProduct.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                }

                _sdb.FeatureValueAssign.Add(FeatureValueAssign);
            }

            _sdb.SaveChanges();

            #endregion

            return true;
        }

        #endregion

        #region edit    --newDon

        public bool EditAlert(long alertId,
                              long userId,
                              string des,
                              bool email,
                              bool sms,
                              bool push,
                              List<SelectItemModel> alertItems)
        {
            #region Validation
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            AlertProduct alertProduct = _sdb.AlertProduct.FirstOrDefault(p => p.UserId == userId && p.Id == alertId);

            if (alertProduct == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            List<CategoryFeature> InsertList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == alertProduct.CategoryId && cf.AvailableADSAlert).ToList();

            string Title = "";
            List<SelectItemModel> ConfirmItems = alertItems.GetConfirmedItems(entityCategoryFeature: InsertList,
                                                                                reqiredCategoryFeature: InsertList.Where(cf => cf.RequiredInADSAlertInsert).ToList(),
                                                                                titleItems: InsertList.Where(cf => cf.AvailableInTitle).ToList(),
                                                                                title: ref Title);
            #endregion

            #region Header

            alertProduct.Title = Title;
            alertProduct.Description = des;
            alertProduct.SendEmail = email;
            alertProduct.SendPush = push;
            alertProduct.SendSMS = sms;
            #endregion

            var ValueAssign = _sdb.FeatureValueAssign.Where(fva => fva.EntityId == alertProduct.Id &&
                                                   fva.EntityType == EntityType.Alert).ToList();


            List<FeatureValueAssignFeatureValueItem> ValueAssignitems = new List<FeatureValueAssignFeatureValueItem>();
            foreach (var item in ValueAssign)
            {
                ValueAssignitems = ValueAssignitems.Concat(item.ListFeatureValue).ToList();
            }

            _sdb.FeatureValueAssignFeatureValueItems.RemoveRange(ValueAssignitems);

            _sdb.FeatureValueAssign.RemoveRange(ValueAssign);
            _sdb.SaveChanges();

            foreach (var item in ConfirmItems)
            {
                FeatureValueAssign FeatureValueAssign;
                if (item.FeatureValueIds.Count > 0)
                {
                    FeatureValueAssign = new FeatureValueAssign
                    {
                        EntityType = EntityType.Alert,
                        CustomValue = "",
                        EntityId = alertProduct.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                    foreach (var fvItem in item.FeatureValueIds)
                    {
                        FeatureValueAssign.ListFeatureValue.Add(new FeatureValueAssignFeatureValueItem
                        {
                            FeatureValueId = fvItem
                        });
                    }

                }
                else
                {
                    FeatureValueAssign = new FeatureValueAssign
                    {
                        EntityType = EntityType.Alert,
                        CustomValue = item.CustomValue,
                        EntityId = alertProduct.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                }

                _sdb.FeatureValueAssign.Add(FeatureValueAssign);
            }

            _sdb.SaveChanges();

            this.UpdateMongoAlert(userId);
            return true;

        }

        #endregion

        #region search    --newDon --notOptimize

        public List<SearchResultItemViewModel> SearchAlertProduct(long categoryId,
                                                                  long userId,
                                                                  List<CFProdoctFilterModel> filters,
                                                                  int type,
                                                                  ref long Count,
                                                                  ref long visitCount,
                                                                  int size = 1,
                                                                  int page = 0)
        {
            long alertId = ProductHelperService.GetUserAlertId(userId: userId, sdb: ref _sdb);

            List<FilterAndCategoryFeatureModel> AppliedFilter = this.ApplyFilterForAggrigate(categoryId: categoryId, filters: filters);

            BsonDocument match = this.GetMongoFilterSearch(categoryId, AppliedFilter);

            List<long> UserAlertVisit = ProductHelperService.UserAlertVisit(userId, alertId, ref _mdb);

            visitCount = UserAlertVisit.Count();

            var MongoResult = _mdb.Products.Find(match).ToList();

            List<SearchResultItemViewModel> model = new List<SearchResultItemViewModel>();

            model = ProductHelperService.ConfigOutPutResult(MongoResult, UserAlertVisit);

            switch ((AlertSearchType)type)
            {
                case AlertSearchType.visit:

                    model = model.Where(m => m.text10).ToList();
                    break;

                case AlertSearchType.notVisit:
                    model = model.Where(m => !m.text10).ToList();
                    break;

                default:
                    break;
            }

            Count = model.Count;

            return model.OrderBy(m => m.advertiseId).Skip(size * page).Take(size).ToList();

        }

        public AlertSingleViewModel AlertSingleView(long userId)
        {
            AlertProduct alertproduct = _sdb.AlertProduct.OrderBy(ap => ap.Id).FirstOrDefault(ap => ap.UserId == userId);

            if (alertproduct == null || alertproduct.Status != AlertProductStatus.فعال)
                return new AlertSingleViewModel();

            AlertSingleViewModel model = new AlertSingleViewModel();

            model.title = alertproduct.Title;
            model.id = alertproduct.Id;
            model.description = alertproduct.Description;
            model.categoryId = alertproduct.CategoryId;

            model.categoryFeature = PublicService.GetEntityAssingeItems(id: alertproduct.Id, type: EntityType.Alert, _sdb: ref _sdb)
                                                 .Select(pcf => new AlertProductCategoryFeatureDetailViewModel
                                                 {
                                                     categoryFeatureId = pcf.categoryFeatureId,
                                                     selectedFeatureValues = pcf.featureValues.Select(i => i.featureValueId).ToList(),
                                                     customValue = pcf.customValue

                                                 }).ToList();

            #region Count
            var AppliedFilter = this.ApplyFilterForAggrigate(categoryId: alertproduct.CategoryId,
                                                   filters: model.categoryFeature.Select(cf => new CFProdoctFilterModel
                                                   {
                                                       CategoryFeatureId = cf.categoryFeatureId,
                                                       CustomValue = cf.customValue,
                                                       FeatureValueId = cf.selectedFeatureValues

                                                   }).ToList());

            BsonDocument match = this.GetMongoFilterSearch(alertproduct.CategoryId, AppliedFilter);

            model.countTotal = _mdb.Products.Count(match);
            model.visitCount = ProductHelperService.UserAlertVisit(userId, alertproduct.Id, ref _mdb).Count(); 

            #endregion

            var LogoBrand = _sdb.FeatureValueAssign.FirstOrDefault(FVA => FVA.CategoryFeature.AvailableFVIcon &&
                                                                      FVA.EntityId == alertproduct.Id && 
                                                                      FVA.EntityType == EntityType.Alert);

            if (LogoBrand != null)
            {
                var brandData = LogoBrand.ListFeatureValue.FirstOrDefault();

                if (brandData != null)
                {
                    Media media = _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue &&
                                                                m.EntityId == brandData.FeatureValueId);

                    if (media != null)
                        model.logoUrl = media.MediaURL;
                }
            }

            return model;
        }


        public ProcuctSearchDetailModel AlertProductDetailSearch(long userId,
                                                                 long productId, 
                                                                 long alertId, 
                                                                 bool isMobile)
        {
            Product product = _sdb.Product.Find(productId);

            if (product == null || product.Status != ProdoctStatus.فعال)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            AlertProduct alertproduct = _sdb.AlertProduct.FirstOrDefault(ap => ap.UserId == userId && ap.Id == alertId);

            if (alertproduct == null || alertproduct.Status != AlertProductStatus.فعال)
                throw new RecabException((int)ExceptionType.AlertNotFound);

            ProcuctSearchDetailModel model = new ProcuctSearchDetailModel();

            model.visit = this.UserProductVisitDetail(userId, product.Id, alertId);
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

            AddUserAlertProductVisit(userId: userId, productId: product.Id, alertId: alertproduct.Id);

            model.confirmDate = product.ConfirmDate.HasValue ? product.ConfirmDate.Value.UTCToPrettyPersian() : "تایید نشده";

            model.email = product.User.Email;

            model.description = product.Description;

            model.categoryId = product.CategoryId;

            model.categoryTitle = product.Category.Title;

            model.tell = product.Tell ?? "";

            model.Id = product.Id;

            model.userName = product.User.FirstName + " " + product.User.LastName;

            model.hasDealership = product.DealershipId.HasValue;

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

            model.advertiseCategoryFeatures = product.ProductFeatures.Select(pcf => new ProductCategoryFeatureDetailViewModel
            {
                title = pcf.CategoryFeature.Title,
                categoryFeatureId = pcf.CategoryFeatureId,
                htmlType = pcf.CategoryFeature.Element.Title,
                featureValues = pcf.ListFeatureValue.Select(pfv => new ProductFeatureValueDetailViewModel
                {
                    featureValueId = pfv.FeatureValueId,
                    title = pfv.FeatureValue.Title
                }).ToList(),
                customValue = pcf.CustomValue != null ? (pcf.CustomValue != "" ? pcf.CustomValue.ToString() : "") : ""


            }).ToList();

            #region reletive
            model.todayPriceSearchFilterItems = product.ProductFeatures.Where(pcf => pcf.CategoryFeature.AvailableTodayPrice)
                                            .Select(pcf => new TodayPriceSearchFilterViewModel
                                            {

                                                categoryFeatureId = pcf.CategoryFeatureId,

                                                featureValueIds = pcf.ListFeatureValue.Select(pfv => pfv.FeatureValueId).ToList(),

                                            }).ToList();

            var categoryFeatureReview = product.ProductFeatures.Where(pcf => pcf.CategoryFeature.AvailableTodayPrice)
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

            long reviewId = 0;


            if (categoryFeatureReview.Count() > 0)
            {
                var filter = categoryFeatureReview.Select(R => new TodayPriceFilterModelWithCategory
                {
                    CategoryFeature = R.CategoryFeature,
                    Filter = new TodayPriceFilterModel
                    {
                        CategoryFeatureId = R.categoryfeatureId,
                        FeatureValueId = R.ListFeatureValue.FirstOrDefault() == null ? 0 : R.ListFeatureValue.FirstOrDefault().featureValueId
                    }
                }).ToList();


                var match = GetMongoFilterSearch(categoryId: product.CategoryId, filter: filter);

                JavaScriptSerializer scr = new JavaScriptSerializer();


                var projection = Builders<BsonDocument>.Projection.Exclude("_id");

                var MongoResult = _mdb.Reviews.Find(match).Project(projection).ToList();

                if (MongoResult.Count > 0)
                {
                    Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(MongoResult.First());

                    reviewId = Convert.ToInt64(data["Id"]);
                }
            }

            model.hasReview = reviewId > 0;

            #endregion

            return model;

        }


        internal long CountAlertForNewProduct(long productId)
        {

            Product product = _sdb.Product.Find(productId);

            List<AlertProduct> Alerts = _sdb.AlertProduct.Where(ap => ap.Status == AlertProductStatus.فعال).ToList();

            long count = 0;
            foreach (var item in Alerts)
            {

                List<FilterAndCategoryFeatureModel> AppliedFilter = this.ApplyFilterForAggrigate(categoryId: product.CategoryId,
                                                                                                 filters: this.GetAlertFilter(item.Id));

                BsonDocument match = this.GetMongoFilterSearchNewProductAlert(AppliedFilter, product.Id);

                if (_mdb.Products.Find(match).Count() > 0)
                { count++; }

            }

            return count;

        }

        #endregion

        #region delete
        public bool ResetAlert(long userId)
        {

            List<AlertProduct> AlertProduct = _sdb.AlertProduct.Where(ap => ap.UserId == userId).ToList();

            foreach (var item in AlertProduct)
            {
                _sdb.FeatureValueAssign.Where(fva => fva.EntityId == item.Id &&
                                               fva.EntityType == EntityType.Alert)
                                 .ToList()
                                 .RemoveAll(i => true);
            }

            _sdb.AlertProduct.RemoveRange(AlertProduct);

            _sdb.SaveChanges();

            this.UpdateMongoAlert(userId);

            return true;

        }

        #endregion

        #region Mongo

        public void AddUserAlertProductVisit(long userId, long productId, long alertId)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "UserId", userId.ToString() } });

            mongoFilter.Add(new BsonDocument { { "ProductId", productId.ToString() } });

            mongoFilter.Add(new BsonDocument { { "AlertId", alertId.ToString() } });

            var filter = new BsonDocument { { "$and", mongoFilter } };
            long count = _mdb.UserAlertVisit.Find(filter: filter).Count();


            if (count == 0)
            {

                BsonDocument newMongoProduct = new BsonDocument {

                { "UserId",userId.ToString() },
                { "ProductId" , productId.ToString() },
                { "AlertId" , alertId.ToString() }

            };

                _mdb.UserAlertVisit.InsertOneAsync(newMongoProduct);
            }
        }

        public bool UserProductVisitDetail(long userId, long productId, long alertId)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "UserId", userId.ToString() } });

            mongoFilter.Add(new BsonDocument { { "ProductId", productId.ToString() } });

            mongoFilter.Add(new BsonDocument { { "AlertId", alertId.ToString() } });

            var filter = new BsonDocument { { "$and", mongoFilter } };

            long count = _mdb.UserAlertVisit.Count(filter: filter);

            return count > 0;

        }

        #endregion

        #region Private

        private List<FilterAndCategoryFeatureModel> ApplyFilterForAggrigate(long categoryId, List<CFProdoctFilterModel> filters)
        {
            List<FilterAndCategoryFeatureModel> AppliedFilter = new List<FilterAndCategoryFeatureModel>();

            List<FilterAndCategoryFeatureModel> JoinCategoryFeatureFilter =
                                filters.Join(_sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId &&
                                                                               cf.AvailableInADS &&
                                                                               cf.AvailableInSearchBox),
                                              f => f.CategoryFeatureId,
                                              cf => cf.Id,
                                              (f, cf) => new FilterAndCategoryFeatureModel
                                              {
                                                  Filter = f,
                                                  CategoryFeature = cf
                                              })
                                              .OrderBy(af => af.CategoryFeature.ParentList.Count())
                                              .ToList();

            foreach (var item in JoinCategoryFeatureFilter)
            {
                #region no custom Value and  cf.count >0

                if (item.Filter.CustomValue == "" && item.Filter.FeatureValueId.Count > 0)
                {
                    #region //if cf have parent

                    if (item.CategoryFeature.ParentList.Count > 0) //if cf have parent
                    {
                        int missdParent = 0;

                        foreach (var parent in item.CategoryFeature.ParentList)
                        {
                            if (!AppliedFilter.Any(ap => ap.Filter.CategoryFeatureId == parent.CategoryFeatureId))
                                missdParent++;
                        }


                        if (missdParent == 0 || item.CategoryFeature.LoadInFirstTime)
                        {
                            int missdFv = 0;

                            foreach (var fv in item.Filter.FeatureValueId)
                            {
                                if (!item.CategoryFeature.FeatureValueList.Any(c => c.Id == fv))
                                    missdFv++;
                            }

                            if (missdFv == 0)
                            {
                                AppliedFilter.Add(item);
                            }
                        }
                    }
                    #endregion

                    #region //if cf have no parent
                    else
                    {
                        int missdFv = 0;

                        foreach (var fv in item.Filter.FeatureValueId)
                        {
                            if (!item.CategoryFeature.FeatureValueList.Any(c => c.Id == fv))
                                missdFv++;
                        }

                        if (missdFv == 0)
                        {
                            AppliedFilter.Add(item);
                        }

                    }
                    #endregion
                }

                #endregion

                if (item.Filter.CustomValue != "")
                {
                    #region //if cf have parent
                    if (item.CategoryFeature.ParentList.Count > 0)
                    {
                        int missdParent = 0;

                        foreach (var parent in item.CategoryFeature.ParentList)
                        {
                            if (!AppliedFilter.Any(ap => ap.Filter.CategoryFeatureId == parent.CategoryFeatureId))
                                missdParent++;
                        }

                        if (missdParent == 0)
                        {
                            AppliedFilter.Add(item);

                        }
                    }
                    #endregion

                    #region //if cf have no parent
                    else
                    {
                        AppliedFilter.Add(item);
                    }
                    #endregion
                }



            }

            return AppliedFilter;
        }


        private BsonDocument GetMongoFilterSearch(long categoryId, List<FilterAndCategoryFeatureModel> filter)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });

            foreach (var item in filter)
            {
                #region customValue
                if (item.Filter.CustomValue != "")
                {
                    string[] temp = item.Filter.CustomValue.Split(',');
                    if (temp.Count() == 2)
                    {
                        BsonArray array = new BsonArray();

                        array.Add(new BsonDocument { { "$gt", "CUM_" + temp[0] } });
                        array.Add(new BsonDocument { { "$lt", "CUM_" + temp[1] } });

                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), new BsonDocument { { "$gt", "CUM_" + temp[0] }, { "$lt", "CUM_" + temp[1] } } } });
                    }
                }
                #endregion

                #region Multi
                else
                {

                    if (item.CategoryFeature.AvailableSearchMultiSelect)
                    {
                        if (item.CategoryFeature.HasMultiSelectValue)//in insert multi
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(fv); }

                            mongoFilter.Add(new BsonDocument {
                            {
                                    "CF_" + item.CategoryFeature.Id.ToString(),new BsonDocument { { "$in", arrayItem } }
                            }});
                        }
                        else //single in insert but multi in view
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), fv.ToString() } }); }

                            if (arrayItem.Count == 1)
                            {
                                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                            }
                            else
                            {
                                mongoFilter.Add(new BsonDocument { { "$or", arrayItem } });
                            }
                        }


                    }
                    else
                    {

                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                    }

                }
                #endregion
            }

            return new BsonDocument { { "$and", mongoFilter } };

        }

        private BsonDocument GetMongoFilterSearchNewProductAlert(List<FilterAndCategoryFeatureModel> filter, long id)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "Id", id.ToString() } });

            foreach (var item in filter)
            {
                #region customValue
                if (item.Filter.CustomValue != null && item.Filter.CustomValue != "")
                {
                    string[] temp = item.Filter.CustomValue.Split(',');
                    if (temp.Count() == 2)
                    {
                        BsonArray array = new BsonArray();

                        array.Add(new BsonDocument { { "$gt", "CUM_" + temp[0] } });
                        array.Add(new BsonDocument { { "$lt", "CUM_" + temp[1] } });

                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), new BsonDocument { { "$gt", "CUM_" + temp[0] }, { "$lt", "CUM_" + temp[1] } } } });
                    }
                }
                #endregion

                #region Multi
                else
                {

                    if (item.CategoryFeature.AvailableSearchMultiSelect)
                    {
                        if (item.CategoryFeature.HasMultiSelectValue)//in insert multi
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(fv); }

                            mongoFilter.Add(new BsonDocument {
                            {
                                    "CF_" + item.CategoryFeature.Id.ToString(),new BsonDocument { { "$in", arrayItem } }
                            }});
                        }
                        else //single in insert but multi in view
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), fv.ToString() } }); }

                            if (arrayItem.Count == 1)
                            {
                                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                            }
                            else
                            {
                                mongoFilter.Add(new BsonDocument { { "$or", arrayItem } });
                            }
                        }


                    }
                    else
                    {

                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                    }

                }
                #endregion
            }

            return new BsonDocument { { "$and", mongoFilter } };

        }


        private List<CFProdoctFilterModel> GetAlertFilter(long alertId)
        {

            List<CFProdoctFilterModel> model = new List<CFProdoctFilterModel>();
            List<FeatureValueAssign> FeatureValueAssigns = _sdb.FeatureValueAssign.Where(fva => fva.EntityType == EntityType.Alert && fva.EntityId == alertId).ToList();

            foreach (var item in FeatureValueAssigns)
            {
                if (item.CategoryFeature.HasCustomValue)
                {
                    model.Add(new CFProdoctFilterModel { CategoryFeatureId = item.CategoryFeatureId, CustomValue = item.CustomValue });

                }
                else
                {
                    model.Add(new CFProdoctFilterModel
                    {
                        CategoryFeatureId = item.CategoryFeatureId,
                        FeatureValueId = item.ListFeatureValue.Select(s => s.FeatureValueId).ToList()
                    });

                }

            }

            return model;
        }

        private BsonDocument GetMongoFilterSearch(long categoryId, List<TodayPriceFilterModelWithCategory> filter)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });

            foreach (var item in filter)
            {
                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.ToString() } });

            }

            return new BsonDocument { { "$and", mongoFilter } };

        }

        private void UpdateMongoAlert(long userId)
        {
            _mdb.UserAlertVisit.DeleteMany(filter: new BsonDocument { { "UserId", userId.ToString() } });

        }

        #endregion

    }
}