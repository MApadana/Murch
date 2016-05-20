using Exon.Recab.Api.Models.Advertise;
using Exon.Recab.Service.Implement.Advertise;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Exon.Recab.Service.Model.ProdoctModel;
using System.Collections.Generic;
using Exon.Recab.Service.Implement.Config;
using System;
using Exon.Recab.Service.Implement.Payment;
using Exon.Recab.Service.Implement.Package;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Implement.Alert;
using Exon.Recab.Api.Models.Advertise.Alert;
using Exon.Recab.Service.Implement.User;
using Exon.Recab.Api.Infrastructure.Filter;
using Exon.Recab.Service.Model.PublicModel;

namespace Exon.Recab.Api.Controllers
{

    public class AdvertiseController : ApiController
    {
        #region init
        public readonly AdvertiseService _prodoctService;

        public readonly BaseConfigService _baseConfigService;

        public readonly PaymentService _PaymentService;

        public readonly VoucherService _voucherService;

        public readonly AlertService _AlertService;

        public readonly UserService _UserService;

        public AdvertiseController()
        {
            _prodoctService = new AdvertiseService();
            _baseConfigService = new BaseConfigService();
            _PaymentService = new PaymentService();
            _voucherService = new VoucherService();
            _AlertService = new AlertService();
            _UserService = new UserService();
        }

        #endregion

        #region Add
        [HttpPost]
        public HttpResponseMessage AddNewAdvertise(AddAdvertiseModel model)
        {

            List<ProductSelectItemModel> productItem = new List<ProductSelectItemModel>();

            bool resultpayment = false;

            foreach (var item in model.advertise.advertiseItems)
            {
                productItem.Add(new ProductSelectItemModel
                {
                    CategoryFeatureId = item.categoryFeatureId,
                    CustomValue = item.customValue,
                    FeatureValueId = item.featureValueIds.Select(fv => fv).ToList()
                });
            }

            List<MediaModel> media = model.advertise.media.Select(m => new MediaModel
            {
                Type = (int)DataType.Picture,
                Url = m.url,
                OrderId = m.orderId

            }).ToList();

            #region HasPackage
            long count = 0;

            if (model.advertise.packageId.HasValue)
            {
                if (!model.exchange.categoryId.HasValue)
                {
                    long productId = _prodoctService.AddNewProdoct(userId: model.userId,
                                                       categoryId: model.advertise.categoryId,                                                       
                                                       description: model.advertise.description,
                                                       exchangeCategoryId: model.exchange.categoryId,
                                                       packageId: model.advertise.packageId.Value,
                                                       productItems: productItem,
                                                       media: media,
                                                       dealershipId: model.advertise.dealershipId,
                                                       tell: model.advertise.tell,
                                                       count :ref count
                                                       );
                    resultpayment = (productId > 0);
                }
                else
                {
                    List<ProductSelectItemModel> exchangeItem = new List<ProductSelectItemModel>();
                    foreach (var item in model.exchange.exchangeItems)
                    {
                        exchangeItem.Add(new ProductSelectItemModel
                        {
                            CategoryFeatureId = item.categoryFeatureId,
                            CustomValue = item.customValue,
                            FeatureValueId = item.featureValueIds.Select(fv => fv).ToList()
                        });
                    }


                    long productId = _prodoctService.AddNewProdoctWithExchange(
                                                      userId: model.userId,
                                                      categoryId: model.advertise.categoryId,
                                                      description: model.advertise.description,
                                                      packageId: model.advertise.packageId.Value,
                                                      productItems: productItem,
                                                      media: media,
                                                      dealershipId: model.advertise.dealershipId,
                                                      tell: model.advertise.tell,
                                                      exchangeCategoryId: model.exchange.categoryId.Value,
                                                      exchangeItems: exchangeItem ,
                                                      count :ref count );

                    resultpayment = (productId > 0);

                }
            }
            #endregion

            #region NO Package
            else
            {
                if (!model.advertise.categoryPurchasePackageTypeId.HasValue || (model.advertise.voucherCode != null && !_voucherService.ValidateVoucher(model.advertise.voucherCode, model.advertise.categoryPurchasePackageTypeId.Value).validate))
                    throw new RecabException(6226);

               

                if (!model.exchange.categoryId.HasValue)
                {
                    long productId = _prodoctService.AddNewProdoct(userId: model.userId,
                                                       categoryId: model.advertise.categoryId,
                                                       description: model.advertise.description,
                                                       exchangeCategoryId: model.exchange.categoryId,
                                                       packageId: model.advertise.packageId,
                                                       productItems: productItem,
                                                       media: media,
                                                       dealershipId: model.advertise.dealershipId,
                                                       tell: model.advertise.tell,
                                                       count :ref count);



                    resultpayment = _PaymentService.InitPayment(userId: model.userId,
                                                       productId: productId,
                                                       bancType: (int)model.advertise.paymentType,
                                                       cpptId: model.advertise.categoryPurchasePackageTypeId.Value,
                                                       voucherCode: model.advertise.voucherCode);


                }
                else
                {
                    List<ProductSelectItemModel> exchangeItem = new List<ProductSelectItemModel>();
                    foreach (var item in model.exchange.exchangeItems)
                    {
                        exchangeItem.Add(new ProductSelectItemModel
                        {
                            CategoryFeatureId = item.categoryFeatureId,
                            CustomValue = item.customValue,
                            FeatureValueId = item.featureValueIds.Select(fv => fv).ToList()
                        });
                    }


                    long productId = _prodoctService.AddNewProdoctWithExchange(
                                                      userId: model.userId,
                                                      categoryId: model.advertise.categoryId,
                                                      description: model.advertise.description,
                                                      packageId: model.advertise.packageId,
                                                      productItems: productItem,
                                                      media: media,
                                                      dealershipId: model.advertise.dealershipId,
                                                      tell: model.advertise.tell,
                                                      exchangeCategoryId: model.exchange.categoryId.Value,
                                                      exchangeItems: exchangeItem ,
                                                      count : ref count);

                    resultpayment = _PaymentService.InitPayment(userId: model.userId,
                                                      productId: productId,
                                                      bancType: (int)model.advertise.paymentType,
                                                      cpptId: model.advertise.categoryPurchasePackageTypeId.Value,
                                                      voucherCode: model.advertise.voucherCode);

                }
               

            }


            return new
            {
                count = count,
                save = resultpayment,
                url = !resultpayment?"": model.advertise.packageId.HasValue ? "" : (model.advertise.paymentType == PaymentType.بانک_ملت ? "http://peyvandha.ir/7-4.htm" : "")
            }.GetHttpResponse();

   
            #endregion
        }

        [HttpPost]
        public HttpResponseMessage AddNewAlert(AddAlertViewModel model)
        {

            List<SelectItemModel> productItems = new List<SelectItemModel>();

            foreach (var item in model.alertItems)
            {
                productItems.Add(new SelectItemModel
                {
                    CategoryFeatureId = item.categoryFeatureId,
                    CustomValue = item.customValue,
                    FeatureValueIds = item.featureValueIds.Select(fv => fv).ToList()
                });
            }

            return _AlertService.AddNewAlert(userId: model.userId,
                                             categoryId: model.categoryId,
                                             des: model.title,
                                             sendEmail: model.sendEmail,
                                             sendSMS: model.sendSMS,
                                             sendPush :model.sendPush,
                                             alertItems: productItems).GetHttpResponse();
        }

        #endregion

        #region search

        [HttpPost]
      //  [CacheEnable("1:0:0")]
        public HttpResponseMessage AdvertiseSearch(AdvertiseSearchModel model)
        {
            long count = 0;

            List<CFProdoctFilterModel> filter = new List<CFProdoctFilterModel>();

            foreach (var item in model.selectedItems)
            {

                filter.Add(new CFProdoctFilterModel
                {
                    CategoryFeatureId = item.categoryFeatureId,
                    CustomValue = item.customValue ?? "",
                    FeatureValueId = item.selectedFeatureValues.Count > 0 ? item.selectedFeatureValues : new List<long>()
                });
            }

            switch (model.searchType)
            {
                case SearhcType.advertise:
                    return _prodoctService.SearchProduct(userId: model.userId,
                                                        sort:(int)model.sortType,
                                                        categoryId: model.categoryId,
                                                        filters: filter,
                                                        type: (int)model.visitType,
                                                        size: model.pageSize,
                                                        page: model.pageIndex,
                                                        packageReletive: false,
                                                        keyword: model.keyword != null ? (model.keyword.Length > 2 ? model.keyword : "") : "",
                                                        Count: ref count).GetHttpResponseWithCount(count);


                case SearhcType.alert:
                    long visit = 0;

                    return _AlertService.SearchAlertProduct(categoryId: model.categoryId,
                                                      userId: model.userId,
                                                      type: (int)model.visitType,
                                                      filters: filter,
                                                      size: model.pageSize,
                                                      page: model.pageIndex,
                                                      Count: ref count,
                                                      visitCount: ref visit).GetHttpResponseWithCount(count);
            }

            throw new RecabException();
        }

        [HttpPost]
     //   [CacheEnable("1:0:0")]
        public HttpResponseMessage AggregationSearch(AdvertiseSearchModel model)
        {
            List<CFProdoctFilterModel> filter = new List<CFProdoctFilterModel>();

            foreach (var item in model.selectedItems)
            {

                filter.Add(new CFProdoctFilterModel
                {
                    CategoryFeatureId = item.categoryFeatureId,
                    CustomValue = item.customValue ?? "",
                    FeatureValueId = item.selectedFeatureValues.Count > 0 ? item.selectedFeatureValues : new List<long>()
                });
            }

            return _prodoctService.AggregateSearchProduct(categoryId: model.categoryId, filters: filter, keyword: model.keyword != null ? (model.keyword.Length > 2 ? model.keyword : "") : "").GetHttpResponse();
        }


        [HttpPost]
        //   [CacheEnable("1:0:0")]
        public HttpResponseMessage AdvertiseLocationSearch(AdvertiseLocationSearchModel model)
        {
            List<CFProdoctFilterModel> filter = new List<CFProdoctFilterModel>();

            foreach (var item in model.selectedItems)
            {

                filter.Add(new CFProdoctFilterModel
                {
                    CategoryFeatureId = item.categoryFeatureId,
                    CustomValue = item.customValue ?? "",
                    FeatureValueId = item.selectedFeatureValues.Count > 0 ? item.selectedFeatureValues : new List<long>()
                });
            }

            long count = 0;
            return _prodoctService.LocationSearchProduct(lat : model.lat ,
                                                        lng :model.lng,
                                                        distance : model.distance,
                                                        categoryId: model.categoryId,
                                                        filters: filter,
                                                        page : model.pageIndex ,
                                                        size : model.pageSize ,
                                                        count : ref count,
                                                        userId : model.userId,
                                                        keyword: model.keyword != null ? (model.keyword.Length > 2 ? model.keyword : "") : "").GetHttpResponseWithCount(count);
        }

        [HttpPost]
 
        public HttpResponseMessage UserAdvertise(UserProductFindModel model)
        {
            long count = 0;

            return _UserService.ListProductForUser(userId: model.cumUserId,
                                                        count: ref count,
                                                        size: model.size,
                                                        skip: model.page).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage AdvertiseDetail(ADFineModel model)
        {
            if (model.alertId.HasValue)
            {

                switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
                {
                    case "mobapi":
                        return _AlertService.AlertProductDetailSearch(userId: model.userId,
                                                                      productId: model.advertiseId,
                                                                      alertId: model.alertId.Value,
                                                                      isMobile: true).GetHttpResponse();
                    case "api":
                        return _AlertService.AlertProductDetailSearch(userId: model.userId,
                                                                      productId: model.advertiseId,
                                                                      alertId: model.alertId.Value,
                                                                      isMobile: false).GetHttpResponse();

                    default:
                        throw new RecabException((int)4000);
                }

            }

            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {
                case "mobapi":
                    return _prodoctService.ProductDetailForSearch(userId: model.userId,
                                                                  productId: model.advertiseId,
                                                                  isMobile: true).GetHttpResponse();
                case "api":
                    return _prodoctService.ProductDetailForSearch(userId: model.userId,
                                                                  productId: model.advertiseId,
                                                                  isMobile: false).GetHttpResponse();
                default:
                    throw new RecabException((int)4000);
            }
        }

        [HttpPost]       
        public HttpResponseMessage DetailUserAlert(ADFineModel model)
        {
            return _AlertService.AlertSingleView(userId: model.userId).GetHttpResponse();
        }

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage PackagePicCount(ADFineModel model)
        {

            return _UserService.PackageMediaCount(model.advertiseId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AdvertiseCompare(ADCompareModel model)
        {
            List<ProcuctSearchDetailModel> result = new List<ProcuctSearchDetailModel>();
            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {
                case "mobapi":
                    foreach (var item in model.advertiseIds)
                    {
                        result.Add(_prodoctService.ProductDetailCompaire(productId: item, categoryId: model.categoryId, isMobile: true));
                    }
                    break;

                case "api":
                    foreach (var item in model.advertiseIds)
                    {
                        result.Add(_prodoctService.ProductDetailCompaire(productId: item, categoryId: model.categoryId, isMobile: true));
                    }
                    break;
                default:
                    throw new Exception("api wrong");
            }

            return result.GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage GetAllFavorite(FaveFindModel model)
        {
            long count = 0;

            return _prodoctService.GetAllFavouriteProduct(userId: model.userId,
                                                          categoryId: model.categoryId,
                                                          count: ref count,
                                                          size: model.pageSize,
                                                          skip: model.pageIndex).GetHttpResponse();
        }

        #endregion

        #region   edit

        [HttpPost]
        public HttpResponseMessage EditAdvertise(EditAdvertiseViewModel model)
        {

            List<ProductSelectItemModel> productItem = new List<ProductSelectItemModel>();

            foreach (var item in model.advertise.advertiseItems)
            {
                productItem.Add(new ProductSelectItemModel
                {
                    CategoryFeatureId = item.categoryFeatureId,
                    CustomValue = item.customValue,
                    FeatureValueId = item.featureValueIds.Select(fv => fv).ToList()
                });
            }

            List<MediaModel> media = model.advertise.media.Select(m => new MediaModel
            {
                Type = (int)DataType.Picture,
                Url = m.url,
                OrderId = m.orderId

            }).ToList();
            if (model.exchange.exchangeItems.Count == 0)
            {
                return (_prodoctService.EditProdoct(productId: model.advertiseId,
                                                    userId: model.userId,
                                                    description: model.advertise.description,
                                                    tell: model.advertise.tell,
                                                    productItems: productItem,
                                                    media: media,
                                                    dealershipId: model.advertise.dealershipId,
                                                    exchangeCategoryId : new long?(),
                                                    isExchange: false) > 0).GetHttpResponse();
            }
            else
            {
                List<ProductSelectItemModel> exchangeItem = new List<ProductSelectItemModel>();
                foreach (var item in model.exchange.exchangeItems)
                {
                    exchangeItem.Add(new ProductSelectItemModel
                    {
                        CategoryFeatureId = item.categoryFeatureId,
                        CustomValue = item.customValue,
                        FeatureValueId = item.featureValueIds.Select(fv => fv).ToList()
                    });
                }

                return _prodoctService.EditProdoctWithExchange(productId: model.advertiseId,
                                                                userId: model.userId,
                                                                description: model.advertise.description,
                                                                tell: model.advertise.tell,
                                                                productItems: productItem,
                                                                media: media,
                                                                exchangeCategoryId: model.exchange.categoryId.Value,
                                                                exchangeItems: exchangeItem,
                                                                dealershipId: model.advertise.dealershipId).GetHttpResponse();
            }


        }

        [HttpPost]
        public HttpResponseMessage EditAlert(EditAlertModel model)
        {

            List<SelectItemModel> productItem = new List<SelectItemModel>();

            foreach (var item in model.alertItems)
            {
                productItem.Add(new SelectItemModel
                {
                    CategoryFeatureId = item.categoryFeatureId,
                    CustomValue = item.customValue,
                    FeatureValueIds = item.featureValueIds.Select(fv => fv).ToList()
                });
            }

            return _AlertService.EditAlert(alertId: model.alertId,
                                           userId: model.userId,
                                           des: model.title,
                                           sms: model.sendSMS,
                                           email:  model.sendEmail,
                                           push : model.sendPush,
                                           alertItems:productItem).GetHttpResponse();


        }

        #endregion

        #region delete

        [HttpPost]

        public HttpResponseMessage ResetAlert(DetailUserAlertModel model)
        {
            return _AlertService.ResetAlert(userId: model.userId).GetHttpResponse();
        }

        [HttpPost]

        public HttpResponseMessage AdvertiseDelete(AdvertiseDeleteModel model)
        {
            return _prodoctService.delete(productId:model.advertiseId).GetHttpResponse();
        }
        #endregion

    }
}
