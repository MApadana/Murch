using Exon.Recab.Api.Models.BaseAdmin;
using Exon.Recab.Api.Models.Public;
using Exon.Recab.Service.Implement.Admin;
using System.Net.Http;
using System.Web.Http;
using Exon.Recab.Infrastructure.Utility.Extension;

namespace Exon.Recab.Api.Controllers
{

    public class BaseAdminController : ApiController
    {
        #region Initiate
        public readonly AdminCategoryService _categoryService;
        

        public BaseAdminController()
        {
            _categoryService = new AdminCategoryService();
        }

        #endregion

        #region Add 
        [HttpPost]
        public HttpResponseMessage AddCategory(AddNewCategoryModel model)
        {
            return _categoryService.AddNewCategory(title: model.title,
                                                   todayPriceRange: model.todayPriceChartlastRange,
                                                   relativeCount :model.relativeCount,
                                                   parentId: model.parentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddCategoryFeature(AddNewCategoryFeatureModel model)
        {

            return _categoryService.AddFeatureToCategory(categoryId: model.pub_CategoryId,
                                                        title: model.pub_Title,
                                                        pattern: model.pub_Pattern,
                                                         ContanerName: model.ads_ContainerName,
                                                        hideContaner: model.ads_HideContainer,
                                                        orderId: model.pub_SearchOrderId,
                                                        advertise: model.ads_AvailableInADS,
                                                        lightSearchAd: model.ads_AvailableInLigthSearch,
                                                        review: model.rew_AvailableInReview,
                                                        searchBoxAd: model.ads_AvailableInSearchBox,
                                                        searchResultAd: model.ads_AvailableInSearchResult,
                                                        loadInFirstTime: model.pub_LoadInFirstTime,
                                                        feedbackAD: model.ads_AvailableInFeedBack,
                                                        article: model.art_AvailableInArticle,
                                                        custom: model.pub_HasCustomValue,
                                                        requiredAD: model.ads_RequiredInInsert,
                                                        multiSelect: model.pub_HasMultiSelectValue,
                                                        exchangeAd: model.ads_AvailableInExchange,
                                                        elementId: model.pub_HtmlElementId,
                                                        androidId: model.pub_AndroidElementId,
                                                        rangeId: model.pub_CategoryFeatureRangeId,
                                                        adTitle: model.ads_AvailableAdTitle,
                                                        adCompaire: model.ads_AvailableInADCompare,
                                                        articleSearch: model.art_AvailableInATsearch,
                                                        articleTitle: model.art_AvailableInATTitle,
                                                        requiredAT: model.art_RequiredInATInsert,
                                                        requiredRv: model.rew_RequiredInRvInsert,
                                                        reviewSearch: model.rew_AvailableInRVSearch,
                                                        searchMulti: model.ads_AvailableInSearchMulti,
                                                        todayPrice: model.tdp_AvailableInTodayPrice,
                                                        adTextSearch: model.ads_AvailableInADTextSearch,
                                                        RvTextSearch: model.rew_AvailableInRVTextSearch,
                                                        aTTextSearch: model.art_AvailableInATTextSearch,
                                                        requierdInTPInsert :model.tdp_RequierdInTPInsert,
                                                        alert:model.ads_AvailableInAlert,
                                                        isMap: model.pub_IsMap,
                                                        isRange: model.pub_IsRang,
                                                        valueSearch: model.pub_HasValueSearch,
                                                        reletiveOrder : model.ads_ReletiveAdsOrder,
                                                        alertInsert : model.ads_RequiredInAlertInsert,
                                                        fvIcon: model.pub_AvailableFVIcon,
                                                        icon: model.pub_AvailableCFIcon,
                                                        reviewTitle: model.rew_AvailableInRVTitle,
                                                        titleOrder: model.pub_TitleOrder.HasValue ? model.pub_TitleOrder.Value : 0,
                                                        todayPriceSearch: model.tdp_AvailableInTPSearch,
                                                        iconUrl: model.pub_IconUrl
                                                         ).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddFeatureValue(AddFeatureValueModel model)
        {

            return _categoryService.AddFeatureValueToCategoryFeature
                                    (categoryFeaturId: model.categoryFeaturId,
                                    description: model.description,
                                     title: model.title,
                                     icon: model.pub_IconUrl,
                                     hideContaner: model.hideContainer,
                                    showcontaner: model.showContainer).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddExchangeCategory(AddExchangeCategoryModel model)
        {

            return _categoryService.AddExchangeCategoryToCategory(categoryId: model.categoryId, categoryExchangeId: model.exchangecategoryId).GetHttpResponse();

        }

        [HttpPost]
        public HttpResponseMessage AddFeatureValueIcon(AddMediaFeatureValueModel model)
        {

            return _categoryService.AddFeatureValueIcon(featureValueId: model.featureValueId, iconUrl: model.iconUrl).GetHttpResponse();
        }

        #endregion

        #region Edit
        [HttpPost]
        public HttpResponseMessage EditCategory(EditCategoryModel model)
        {

            return _categoryService.EditCategory(categoryId: model.categoryId,
                                                 title: model.title,
                                                 todayPriceRange: model.todayPriceChartlastRange,
                                                 relativeCount: model.relativeCount,
                                                 parentId: model.parentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage EditCategoryFeature(EditCategoryFeaturModel model)
        {


            return _categoryService.EditCategoryFeature(categoryFeatureId: model.categoryFeatureId,
                                                        title: model.pub_Title,
                                                        pattern: model.pub_Pattern,
                                                        ContanerName: model.ads_ContainerName,
                                                        hideContaner: model.ads_HideContainer,
                                                        orderId: model.pub_SearchOrderId,
                                                        advertise: model.ads_AvailableInADS,
                                                        lightSearchAd: model.ads_AvailableInLigthSearch,
                                                        review: model.rew_AvailableInReview,
                                                        searchBoxAd: model.ads_AvailableInSearchBox,
                                                        searchResultAd: model.ads_AvailableInSearchResult,
                                                        loadInFirstTime: model.pub_LoadInFirstTime,
                                                        feedbackAD: model.ads_AvailableInFeedBack,
                                                        article: model.art_AvailableInArticle,
                                                        custom: model.pub_HasCustomValue,
                                                        requiredAD: model.ads_RequiredInInsert,
                                                        multiSelect: model.pub_HasMultiSelectValue,
                                                        exchangeAd: model.ads_AvailableInExchange,
                                                        elementId: model.pub_HtmlElementId,
                                                        androidId: model.pub_AndroidElementId,
                                                        rangeId: model.pub_CategoryFeatureRangeId,
                                                        adTitle: model.ads_AvailableAdTitle,
                                                        adCompaire: model.ads_AvailableInADCompare,
                                                        articleSearch: model.art_AvailableInATsearch,
                                                        articleTitle: model.art_AvailableInATTitle,
                                                        requiredAT: model.art_RequiredInATInsert,
                                                        requiredRv: model.rew_RequiredInRvInsert,
                                                        reviewSearch: model.rew_AvailableInRVSearch,
                                                        searchMulti: model.ads_AvailableInSearchMulti,
                                                        todayPrice: model.tdp_AvailableInTodayPrice,
                                                        adTextSearch: model.ads_AvailableInADTextSearch,
                                                        RvTextSearch: model.rew_AvailableInRVTextSearch,
                                                        aTTextSearch: model.art_AvailableInATTextSearch,
                                                        alert : model.ads_AvailableInAlert,
                                                        reletiveOrder : model.ads_ReletiveAdsOrder,
                                                         requierdAlertInsert : model.ads_RequiredInAlertInsert,
                                                        isMap: model.pub_IsMap,
                                                        isRange: model.pub_IsRang,
                                                        valueSearch: model.pub_HasValueSearch,
                                                        requierdInTPInsert :model.tdp_RequierdInTPInsert,
                                                        fvIcon: model.pub_AvailableFVIcon,
                                                        icon: model.pub_AvailableCFIcon,
                                                        reviewTitle: model.rew_AvailableInRVTitle,
                                                        titleOrder: model.pub_TitleOrder.HasValue ? model.pub_TitleOrder.Value : 0,
                                                        todayPriceSearch: model.tdp_AvailableInTPSearch,
                                                        iconUrl: model.pub_IconUrl
                                                        ).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage EditFeatureValue(EditFeatureValueModel model)
        {

            return _categoryService.EditFeatureValue(featureValueId: model.id,
                                                     description: model.description,
                                                     title: model.title,
                                                     icon: model.pub_IconUrl,
                                                     hideContaner: model.hideContainer,
                                                     showcontaner: model.showContainer).GetHttpResponse();

        }

        #endregion

        #region Report

        [HttpPost]
        public HttpResponseMessage ListCategory(ListCategoryModel model)
        {
            long count = 0;
            return _categoryService.GetAllCategoris(title: "",
                                                    count: ref count,
                                                    size: model.pageSize,
                                                    skip: model.pageIndex).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ListCategoryFeature(FindModel model)
        {
            int count = 0;
            return _categoryService.GetAllCategoryFeatureFromCategory(title: model.title ?? "",
                                                                      parentCategoryId: model.id,
                                                                      count: ref count,
                                                                      size: model.pageSize,
                                                                       skip: model.pageIndex).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ListFeatureValue(FindModel model)
        {
            int count = 0;
            return _categoryService.GetAllFeatureValueFromCategoryFeature(title: model.title ?? "",
                                                                              parentCategoryFeatureId: model.id,
                                                                              count: ref count,
                                                                              size: model.pageSize,
                                                                              skip: model.pageIndex).GetHttpResponseWithCount(count);
        }


        [HttpPost]
        public HttpResponseMessage ListAndroidElement()
        {
            return _categoryService.GetListAndroidElement().GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ListHtmlElement()
        {
            return _categoryService.GetListHtmlElement().GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ListCategoryExchange(CategoryExchangeFindModel model)
        {
            int count = 0;
            return _categoryService.GetAllExchangeCategoryFromCategory(model.categoryId, ref count, model.pageSize, model.pageIndex).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage ListCategoryFavorite(BaseAdminFavFindModel model)
        {
            return _categoryService.GetAllFavoriteUserCategory(userId: model.userId).GetHttpResponse();
        }
        #endregion

        #region Delete
        [HttpPost]
        public HttpResponseMessage DeleteCategory(BaseAdminDeleteModel model)
        {
            return _categoryService.DeleteCategory(model.id).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteCategoryFeature(BaseAdminDeleteModel model)
        {
            return _categoryService.DeleteCategoryFeature(model.id).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteFeature(BaseAdminDeleteModel model)
        {
            return _categoryService.DeleteFeatureValue(model.id).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteExchangeCategory(BaseAdminDeleteModel model)
        {

            return _categoryService.DeleteExchangeFromCategory(model.id).GetHttpResponse();

        }

        #endregion

        #region cf Dependency

        [HttpPost]
        public HttpResponseMessage AddCategoryFeatureParent(AddCategoryFeatureParentModel model)
        {

            return _categoryService.AddParentToCategoryFeature(model.categoryfeatureId, model.categoryfeatureParentId).GetHttpResponse();

        }

        [HttpPost]
        public HttpResponseMessage AddCategoryFeatureHide(AddCategoryFeatureHideModel model)
        {

            return _categoryService.AddHideToCategoryFeature(model.categoryfeatureId, model.categoryfeatureHideId).GetHttpResponse();

        }

        [HttpPost]
        public HttpResponseMessage AddCategoryFeatureDefault(AddCategoryFeatureDefaultModel model)
        {
            return _categoryService.AddCategoryFeatureDefault(id: model.id,
                                                             cfDefaultId: model.categoryFeatureId,
                                                             enable: model.isEnable,
                                                             customValue: model.customValue,
                                                             featureValueId: model.featureValueId).GetHttpResponse();

        }

        [HttpPost]
        public HttpResponseMessage ListCategoryFeatureChildren(FindModel model)
        {
            int count = 0;
            return _categoryService.GetChildrenOfCategoryFeature(model.title ?? "",
                                                                model.id,
                                                                ref count,
                                                                size: model.pageSize,
                                                                skip: model.pageIndex).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ListCategoryFeatureParents(FindModel model)
        {
            int count = 0;
            return _categoryService.GetParentOfCategoryFeature(model.title ?? "", model.id, ref count, size: model.pageSize, skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage ListCategoryFeatureHide(FindModel model)
        {
            int count = 0;
            return _categoryService.GetHideOfCategoryFeature(model.title ?? "",
                                                                model.id,
                                                                ref count,
                                                                size: model.pageSize,
                                                                skip: model.pageIndex).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ListCategoryFeatureDefault(DefaultFindModel model)
        {
            int count = 0;
            return _categoryService.GetCategoryFeatureDefault( id :model.id ,enable :model.isEnable ,count: ref count, size: model.pageSize, skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage DeleteCategoryFeatureDependency(DependencyDeleteModel model)
        {
            return _categoryService.DeleteCategoryFeatureDependency(model.id, model.parentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteCategoryFeatureHide(DeleteCategoryFeatureHideModel model)
        {
            return _categoryService.DeleteCategoryFeatureHide(model.id, model.hideId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteCategoryFeatureDefault(DeleteCategoryFeatureDefultModel model)
        {
            return _categoryService.DeleteCategoryFeatureDefaultValue(id:model.id,cfDefaultId: model.categoryFeatureId , enable:model.isEnable).GetHttpResponse();
        }


        #endregion

        #region fv Dependency

        [HttpPost]
        public HttpResponseMessage AddFeatureValueParent(AddFeatureValueParentModel model)
        {
            return _categoryService.AddParentToFeatureValue(model.featureValueId, model.featureValueParentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddFeatureValueHide(AddFeatureValueParentModel model)
        {
            return _categoryService.AddHideToFeatureValue(model.featureValueId, model.featureValueParentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddFeatureValueShow(AddFeatureValueParentModel model)
        {

            return _categoryService.AddShowToFeatureValue(model.featureValueId, model.featureValueParentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddFeatureValueDefault(AddCategoryFeatureDefaultModel model)
        {
            return _categoryService.AddFeatureValueDefault(id: model.id,
                                                             cfDefaultId: model.categoryFeatureId,
                                                             enable: model.isEnable,
                                                             customValue: model.customValue,
                                                             featureValueId: model.featureValueId).GetHttpResponse();

        }


        [HttpPost]
        public HttpResponseMessage ListFeatureValueChildren(FindModel model)
        {
            int count = 0;
            var temp = _categoryService.GetChildrenOfFeatureValue(featureValueId: model.id, title: model.title ?? "", count: ref count, size: model.pageSize, skip: model.pageIndex);

            return temp.GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ListFeatureValueHide(FindModel model)
        {
            int count = 0;
            var temp = _categoryService.GetHideOfFeatureValue(featureValueId: model.id, title: model.title ?? "", count: ref count, size: model.pageSize, skip: model.pageIndex);

            return temp.GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ListFeatureValueShow(FindModel model)
        {
            int count = 0;
            var temp = _categoryService.GetShowOfFeatureValue(featureValueId: model.id, title: model.title ?? "", count: ref count, size: model.pageSize, skip: model.pageIndex);

            return temp.GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ListFeatureValueParents(FindModel model)
        {
            int count = 0;
            var temp = _categoryService.GetParentOfFeatureValue(title: model.title ?? "",
                                                                FeatureValueId: model.id,
                                                                count: ref count,
                                                                size: model.pageSize,
                                                                skip: model.pageIndex);

            return temp.GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage ListFeatureValueDefault(DefaultFindModel model)
        {
            int count = 0;
            return _categoryService.GetFeatureValueDefault(id: model.id, enable: model.isEnable, count: ref count, size: model.pageSize, skip: model.pageIndex).GetHttpResponseWithCount(count);
        }


        [HttpPost]
        public HttpResponseMessage DeleteFeatureValueDependency(DependencyDeleteModel model)
        {
            return _categoryService.DeleteFeatureValueDependency(model.id, model.parentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteFeatureValueHide(DependencyDeleteModel model)
        {
            return _categoryService.DeleteFeatureValueHide(model.id, model.parentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteFeatureValueshow(DependencyDeleteModel model)
        {
            return _categoryService.DeleteFeatureValueShow(model.id, model.parentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteFeatureValueDefault(DeleteCategoryFeatureDefultModel model)
        {
            return _categoryService.DeleteFeatureValueDefaultValue(id: model.id, cfDefaultId: model.categoryFeatureId, enable: model.isEnable).GetHttpResponse();
        }
        #endregion

    }
}
