using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Exon.Recab.Service.Implement.Config;
using Exon.Recab.Api.Models.BaseConfig;
using Exon.Recab.Service.Model.ConfigModel;
using Exon.Recab.Api.Models.Public;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.User;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Implement.Admin;
using Exon.Recab.Service.Implement.FeedBack;
using Exon.Recab.Api.Infrastructure.Filter;

namespace Exon.Recab.Api.Controllers
{

    public class BaseConfigController : ApiController
    {
        #region init
        private readonly BaseConfigService _BaseConfigService;

        private readonly UserService _userService;

        private readonly AdminCategoryService _adminService;

        private readonly FeedBackService _FeedbackService;

        public BaseConfigController()
        {
            _BaseConfigService = new BaseConfigService();
            _userService = new UserService();
            _adminService = new AdminCategoryService();
            _FeedbackService = new FeedBackService();
        }

        #endregion

        #region   featureValue
        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage SimpleSearchFeatureValue(SimpleSearchFeatureValueFilterModel model)
        {

            return _BaseConfigService.GetBaseFeatureValue(categoryId: model.categoryId,
                                                            type: (int)model.searchType,
                                                            isSimple: model.isSimple,
                                                            categoryFeatureFilter: model.selectedItems.Select(c => new CategoryFeatureFilterModel
                                                            {
                                                                CategoryFeatureId = Convert.ToInt64(c.categoryFeatureId),
                                                                FeatureValueId = c.featureValueIds.Select(k => Convert.ToInt64(k)).ToList()

                                                            }).ToList()
                                                            ).GetHttpResponse();

        }


        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage ManageFeatureValue(SimpleSearchFeatureValueFilterModel model)
        {

            return _BaseConfigService.ManageFeatureValue(categoryId: model.categoryId,
                                                            type: (int)model.searchType,
                                                            categoryFeatureFilter: model.selectedItems.Select(c => new CategoryFeatureFilterModel
                                                            {
                                                                CategoryFeatureId = Convert.ToInt64(c.categoryFeatureId),
                                                                FeatureValueId = c.featureValueIds.Select(k => Convert.ToInt64(k)).ToList()

                                                            }).ToList()
                                                            ).GetHttpResponse();

        }

        #endregion
        
        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage InitialSearchFeatureValue(SimpleSearchFeatureValueFilterModel model)
        {

            return _BaseConfigService.GetInitSearchFeatureValue(categoryId: model.categoryId,
                                                            type: (int)model.searchType,
                                                            isSimple: model.isSimple,
                                                            isMobile: Request.RequestUri.Segments[1].Replace("/", "").ToLower() =="mobapi",
                                                            selectedItems: model.selectedItems.Select(c => new CategoryFeatureFilterModel
                                                            {
                                                                CategoryFeatureId = Convert.ToInt64(c.categoryFeatureId),
                                                                FeatureValueId = c.featureValueIds.Select(k => Convert.ToInt64(k)).ToList()

                                                            }).ToList()).GetHttpResponse();

        }

        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage SearchFeatureValue(SimpleSearchFeatureValueFilterModel model)
        {

            return _BaseConfigService.SearchFeatureValue(categoryId: model.categoryId,
                                                            type: (int)model.searchType,
                                                            isSimple: model.isSimple,
                                                            categoryFeatureFilter: model.selectedItems.Select(c => new CategoryFeatureFilterModel
                                                            {
                                                                CategoryFeatureId = Convert.ToInt64(c.categoryFeatureId),
                                                                FeatureValueId = c.featureValueIds.Select(k => Convert.ToInt64(k)).ToList()

                                                            }).ToList()).GetHttpResponse();

        }

        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage GetSearchConfig(SearchConfig model)
        {

            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {

                case "mobapi":
                    return _BaseConfigService.GetConfigForSearch(model.categoryId, (int)model.searchType, model.isSimple, true).GetHttpResponse();

                case "api":
                    return _BaseConfigService.GetConfigForSearch(model.categoryId, (int)model.searchType, model.isSimple, false).GetHttpResponse();

                default:
                    throw new Exception();

            }

        }

     
        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage GetAggregationSearchConfig(SearchConfig model)
        {

            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {

                case "mobapi":
                    return _BaseConfigService.GetConfigForSearchAggregation(model.categoryId, (int)model.searchType, true).GetHttpResponse();

                case "api":
                    return _BaseConfigService.GetConfigForSearchAggregation(model.categoryId, (int)model.searchType, false).GetHttpResponse();

                default:
                    return new HttpResponseMessage();

            }

        }

        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage GetManagementConfig(ManageConfig model)
        {

            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {

                case "mobapi":
                    return _BaseConfigService.GetConfigForManagment(model.categoryId, (int)model.searchType, true).GetHttpResponse();

                case "api":
                    return _BaseConfigService.GetConfigForManagment(model.categoryId, (int)model.searchType, false).GetHttpResponse();

                default:
                    return new HttpResponseMessage();
            }


        }


        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage GetExchangeManagementConfig(ManageConfig model)
        {

            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {

                case "mobapi":
                    return _BaseConfigService.GetExchangeConfigForManagment(model.categoryId, (int)model.searchType, true).GetHttpResponse();

                case "api":
                    return _BaseConfigService.GetExchangeConfigForManagment(model.categoryId, (int)model.searchType, false).GetHttpResponse();

                default:
                    throw new RecabException(HttpStatusCode.NotFound.ToString(), HttpStatusCode.NotFound);

            }


        }


        [HttpPost]
        public HttpResponseMessage GetAdvertiseEditConfig(AdvertiseEditConfig model)
        {

            return _BaseConfigService.InitEditAdvertise(userId: model.userId, advertiseId: model.advertiseId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage GetProductAdminEditConfig(ProductAdminEditConfig model)
        {

            return _BaseConfigService.InitEditAdvertise(userId: model.cumUserId, advertiseId: model.productId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage GetAlertEditConfig(AlertEditConfigModel model)
        {

            return _BaseConfigService.InitEditAlert(userId: model.userId, alertId: model.alertId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage GetExchangeAdvertiseEditConfig(AdvertiseEditConfig model)
        {

            return _BaseConfigService.InitEditProductExchange(userId: model.userId, productId: model.advertiseId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage GetExchangeProductAdminEditConfig(ProductAdminEditConfig model)
        {

            return _BaseConfigService.InitEditProductExchange(userId: model.cumUserId, productId: model.productId).GetHttpResponse();
        }
       
        [HttpPost]
        [CacheEnable("10:0:0")]
        public HttpResponseMessage GetFeedbackConfig(FeedbackConfig model)
        {

            return _FeedbackService.GetAllFeedbackCategoryFeature(model.categoryId).GetHttpResponse();
        }
    }
}
