using Exon.Recab.Api.Models.AdminManageModel;
using Exon.Recab.Api.Models.Advertise;
using Exon.Recab.Api.Models.Article;
using Exon.Recab.Api.Models.Review;
using Exon.Recab.Api.Models.TodayPrice;
using Exon.Recab.Api.Models.User;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.Admin;
using Exon.Recab.Service.Implement.Advertise;
using Exon.Recab.Service.Implement.ArTicl;
using Exon.Recab.Service.Implement.ArtTicl;
using Exon.Recab.Service.Implement.ReView;
using Exon.Recab.Service.Implement.ToDayPrice;
using Exon.Recab.Service.Implement.User;
using Exon.Recab.Service.Model.ArticleModel;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.PublicModel;
using Exon.Recab.Service.Model.ReviewModel;
using Exon.Recab.Service.Model.TodayPriceModel;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Exon.Recab.Api.Controllers
{
    public class AdminManageController : ApiController
    {
        #region init
        private UserService _userService;
        private DealershipService _dealershipService;
        private AdvertiseService _prodoctService;//ForEditProduct
        private AdminAdvertiseService _adminAdvertiseService;
        private AdminUserService _adminUserService;
        private ReviewService _ReviewService;
        private TodayPriceService _TodayPriceService;
        public ArticleService _ArticleService;
        public ArticleStructureService _ArticleStructureService;

        public AdminManageController()
        { } 
        #endregion

        #region Dealership

        [HttpPost]
        public HttpResponseMessage AddDealership(AdminAddDealershipModel model)
        {
            _dealershipService = new DealershipService();

            return _dealershipService.AddDelership(address: model.address,
                                             userId: model.cumUserId,
                                             title: model.title,
                                             fax: model.fax,
                                             coordinateLat: model.lat,
                                             coordinateLong: model.lng,
                                             description: model.description,
                                             websiteUrl: model.websiteUrl,
                                             logoUrl: model.logoUrl,
                                             tell: model.tell,
                                             cityId: model.cityId,
                                             categoryId: model.categoryId).GetHttpResponse();


        }

        [HttpPost]
        public HttpResponseMessage changeStatusDealership(DealershipAdminChangeStatusModel model)
        {
            _dealershipService = new DealershipService();

            return _dealershipService.ChangeStatus(dealershipId: model.dealershipId, status: (int)model.status).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ListDealership(DealershipAdminFindModel model)
        {
            _dealershipService = new DealershipService();

            long count = 0;

            return _dealershipService.ListDelershipAdmin(title: model.title ?? "",
                                                         categoryId: model.categoryId,
                                                         status: (int)model.status,
                                                         skip: model.pageIndex,
                                                         size: model.pageSize,
                                                         count: ref count,
                                                         cityId: model.cityId,
                                                         stateId: model.stateId).GetHttpResponseWithCount(count);
        }


        [HttpPost]
        public HttpResponseMessage EditDealership(AdminEditDealershipModel model)
        {
            _dealershipService = new DealershipService();

            return _dealershipService.AdminEditDelership(cumUserId: model.cumUserId,
                                                          dealershipId: model.dealershipId,
                                                          title: model.title,
                                                          address: model.address,
                                                          tell: model.tell,
                                                          fax: model.fax,
                                                          coordinateLat: model.lat,
                                                          coordinateLong: model.lng,
                                                          description: model.description,
                                                          websiteUrl: model.websiteUrl,
                                                          logoUrl: model.logoUrl,
                                                          cityId: model.cityId,
                                                          status: (int)model.status,
                                                          categoryId: model.categoryIds).GetHttpResponse();
        }


        [HttpPost]
        public HttpResponseMessage GetDealershipById(AdminFindDealershipByIdModel model)
        {
            _dealershipService = new DealershipService();
            return _dealershipService.GetByID(model.dealershipid).GetHttpResponse();
        }

        #endregion

        #region Advertise
        [HttpPost]
        public HttpResponseMessage ListAdvertise(ListProductModel model)
        {
            _adminAdvertiseService = new AdminAdvertiseService();

            long count = 0;

            return _adminAdvertiseService.ListProductByCategoryStatus(categoryId: model.categoryId,
                                                                        status: (int)model.status,
                                                                        feedback: (int)model.feedBack,
                                                                        dealership: (int)model.dealership,
                                                                        count: ref count,
                                                                        size: model.pageSize,
                                                                        skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage ChangeStatusAdvertise(ChangeStatusProductModel model)
        {
            _adminAdvertiseService = new AdminAdvertiseService();

            return _adminAdvertiseService.ProductChangeStatus(productId: model.advertiseId, status: (int)model.status).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage FeedBackAdvertise(FeedBackProductFindModel model)
        {
            _adminAdvertiseService = new AdminAdvertiseService();
            long count = 0;
            return _adminAdvertiseService.ProductFeedBack(productId: model.advertiseId,
                                                          count: ref count,
                                                          size: model.pageSize,
                                                          skip: model.pageIndex).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage EditAdvertiseUser(AdminEditAdvertiseModel model)
        {
            _prodoctService = new AdvertiseService();

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
                                                       userId: model.cumUserId,
                                                       description: model.advertise.description,
                                                       tell: model.advertise.tell,
                                                       productItems: productItem,
                                                       media: media,
                                                       exchangeCategoryId : model.exchange.categoryId,
                                                       dealershipId: model.advertise.dealershipId,
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
                                                                userId: model.cumUserId,
                                                                description: model.advertise.description,
                                                                tell: model.advertise.tell,
                                                                productItems: productItem,
                                                                media: media,
                                                                exchangeCategoryId: model.exchange.categoryId.Value,
                                                                exchangeItems: exchangeItem,
                                                                dealershipId: model.advertise.dealershipId).GetHttpResponse();
            }

        }

        #endregion

        #region User

        [HttpPost]
        public HttpResponseMessage PackagebuyAdminDetail(AdminDetailPackagebuyModel model)
        {
            _adminUserService = new AdminUserService();

            long count = 0;

            return _adminUserService.PackageBuyAdminDetail(toPersianData: model.toPersianData,
                                                                 fromPersianData: model.fromPersianData,
                                                                 count: ref count,
                                                                 size: model.pageSize,
                                                                 skip: model.pageIndex).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ChangeUserPackageCreditStatus(ChangeUserPackageCreditStatusModel model)
        {
            _adminUserService = new AdminUserService();

            return _adminUserService.ChangeUserPackageCreditStatus(upcId: model.userPackageCreditId, status: (int)model.status).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddUser(SingUpModel model)
        {
            _userService = new UserService();

            return _userService.AddUser(username: model.email,
                                                 password: model.password,
                                                 fname: model.firstName,
                                                 lname: model.lastName,
                                                 mobile: model.mobile,
                                                 type: (int)model.genderType).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddUserbyRole(SingUpByRoleModel model)
        {
            _userService = new UserService();

            return _userService.SignUpByRole(username: model.email,
                                                password: model.password,
                                                fname: model.firstName,
                                                lname: model.lastName,
                                                mobile: model.mobile,
                                                type: (int)model.genderType,
                                                roleId: model.roleId).GetHttpResponse();
        }

        #endregion

        #region Review

        #region Add
        [HttpPost]
        public HttpResponseMessage AddReview(AddReviewModel model)
        {
            _ReviewService = new ReviewService();

            List<SelectItemModel> selectItems = new List<SelectItemModel>();

            foreach (var item in model.selectItem)
            {
                selectItems.Add(new SelectItemModel { CategoryFeatureId = item.categoryFeatureId, FeatureValueIds = item.featureValueId });
            }

            List<MediaModel> media = model.media != null ? model.media.Select(m => new MediaModel
            {
                Type = (int)DataType.Picture,
                Url = m.url,
                OrderId = m.orderId

            }).ToList() : new List<MediaModel>();



            return _ReviewService.AddNewReview(userId: model.userId,
                                                categoryId: model.categoryId,
                                                body: model.body,
                                                reviewItem: selectItems,
                                                media: media).GetHttpResponse();

        }
       
        #endregion

        #region edit
        [HttpPost]
        public HttpResponseMessage EditReview(EditReviewModel model)
        {
            _ReviewService = new ReviewService();

            List<MediaModel> media = model.media != null ? model.media.Select(m => new MediaModel
            {
                Type = (int)DataType.Picture,
                Url = m.url,
                OrderId = m.orderId

            }).ToList() : new List<MediaModel>();


            return _ReviewService.EditReview(reviewId: model.reviewId,
                                             body: model.body,
                                             media: media).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AdminInitEditReview(FindSingleReviewModel model)
        {
            _ReviewService = new ReviewService();
            return _ReviewService.InitEditReview(reviewId: model.reviewId).GetHttpResponse();
        }


        #endregion

        #region delete

        [HttpPost]
        public HttpResponseMessage RemoveReview(FindSingleReviewModel model)
        {
            _ReviewService = new ReviewService();
            return _ReviewService.RemoveReview(reviewId: model.reviewId).GetHttpResponse();

        }
        #endregion

        #region   report


        [HttpPost]
        public HttpResponseMessage ListReview(ReviewFindModel model)
        {
            _ReviewService = new ReviewService();
            long count = 0;
            return _ReviewService.GetALLReview(model.categoryId, ref count, model.pageSize, model.pageIndex).GetHttpResponseWithCount(count);

        }

        #endregion
        #endregion

        #region TodayPrice

        #region ADD
        [HttpPost]

        public HttpResponseMessage AddTodayPrice(AddTodayPriceModel model)
        {
            _TodayPriceService = new TodayPriceService();
            List<SelectItemModel> feature = new List<SelectItemModel>();

            foreach (var item in model.selectedItems)
            {

                feature.Add(new SelectItemModel
                {
                    CategoryFeatureId = item.categoryFeatureId,
                    CustomValue = item.customValue,
                    FeatureValueIds = item.featureValueIds.ToList()
                });
            }

            return _TodayPriceService.AddNewTodayPriceConfig(userId: model.userId,
                                                            categoryId: model.categoryId,
                                                            sellOption: model.sellOption,
                                                            persianLastUpdateDate: model.persianLastUpdateDate,
                                                            price: model.price,
                                                            dealershipPrice: model.dealershipPrice,
                                                            todayPriceItem: feature).GetHttpResponse();
        }

        [HttpPost]

        public HttpResponseMessage AddTodayPriceOption(AddTodayPriceOptionModel model)
        {
            _TodayPriceService = new TodayPriceService();
            return _TodayPriceService.AddTodayPriceOption(categoryId: model.categoryId, title: model.title).GetHttpResponse();
        }


        #endregion

        #region edit
        [HttpPost]

        public HttpResponseMessage EditTodayPriceOption(EditTodayPriceOptionModel model)
        {
            _TodayPriceService = new TodayPriceService();
            return _TodayPriceService.EditTodayPriceOption(todayPriceOptionId: model.id, categoryId: model.categoryId, title: model.title).GetHttpResponse();
        }


        [HttpPost]
        public HttpResponseMessage InitEditTodayPrice(TodayPriceFindModel model)
        {
            _TodayPriceService = new TodayPriceService();
            return _TodayPriceService.InitEditTodayPrice(todayPriceId: model.todayPriceId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage UpdatePrice(TodayPriceUpdateModel model)
        {
            _TodayPriceService = new TodayPriceService();

            return _TodayPriceService.UpdatePrice(todayPriceId: model.todayPriceId,
                                                    price: model.price,
                                                    dealershipPrice: model.dealershipPrice,
                                                    persianUpdateTime: model.persianDate).GetHttpResponse();
        }

        #endregion

        #region Delete

        [HttpPost]
        public HttpResponseMessage DeleteTodayPriceOption(DeleteTodayPriceModel model)
        {
            _TodayPriceService = new TodayPriceService();
            return _TodayPriceService.DeleteTodayPriceOption(todayPriceOptionId: model.id).GetHttpResponse();
        }


        [HttpPost]
        public HttpResponseMessage DeleteTodayPrice(TodayPriceFindModel model)
        {
            _TodayPriceService = new TodayPriceService();

            return _TodayPriceService.DeleteTodayPrice(model.todayPriceId).GetHttpResponse();

        }

        [HttpPost]
        public HttpResponseMessage DeletePriceHistory(DeleteTodayPriceHistoryModel model)
        {
            _TodayPriceService = new TodayPriceService();
            return _TodayPriceService.DeletePriceHistory(historyId: model.historyId).GetHttpResponse();
        }


        #endregion

        #region Report
        [HttpPost]
        public HttpResponseMessage ListTodayPriceOption(ListTodayPriceOptionModel model)
        {
            _TodayPriceService = new TodayPriceService();
            long count = 0;

            return _TodayPriceService.TodayOptionList(model.categoryId, ref count, model.pageSize, model.pageIndex).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage TodayPriceSearch(TodayPriceSearchModel model)
        {
            _TodayPriceService = new TodayPriceService();
            long count = 0;

            List<TodayPriceFilterModel> filter = new List<TodayPriceFilterModel>();

            foreach (var item in model.selectedItems)
            {
                filter.Add(new TodayPriceFilterModel { CategoryFeatureId = item.categoryFeatureId, FeatureValueId = (item.featureValueIds.Count > 0 ? item.featureValueIds.FirstOrDefault() : new long()) });

            }

            return _TodayPriceService.TodayPriceSearch(categoryId: model.categoryId,
                                                        keyword : model.keyword,
                                                       count: ref count,
                                                       size: model.pageSize,
                                                       skip: model.pageIndex,
                                                       filter: filter).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage TodayPriceHistorySearch(TodayPriceFindModel model)
        {
            _TodayPriceService = new TodayPriceService();
            return _TodayPriceService.GetAllTodayPriceHistory(model.todayPriceId).GetHttpResponse();
        }


        #endregion

        #endregion

        #region Article

        #region add

        [HttpPost]
        public HttpResponseMessage AddArticleStructure(AddArticleStructureModel model)
        {
            _ArticleStructureService = new ArticleStructureService();
            return _ArticleStructureService.AddArticleStructure(categoryId: model.categoryId,
                                                                title: model.title,
                                                                parentId: model.parentArticleStructureId,
                                                                logoUrl: model.logoUrl).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddArticle(AddArticleModel model)
        {
            _ArticleService = new ArticleService();
            List<SelectItemModel> selectItems = new List<SelectItemModel>();

            foreach (var item in model.selectedItems)
            {
                selectItems.Add(new SelectItemModel { CategoryFeatureId = item.categoryFeatureId, FeatureValueIds = item.featureValueIds });
            }

            return _ArticleService.AddNewArticle(userId: model.userId,
                                                title: model.title,
                                                articleStructureId: model.articleStructureId,
                                                briefdes: model.briefDescription,
                                                body: model.htmlContent,
                                                logoUrl: model.logoUrl,
                                                articleItem: selectItems).GetHttpResponse();

        }

        #endregion

        #region report


        [HttpPost]
        public HttpResponseMessage GetAllArticleStructure(ArticleStructureSearchModel model)
        {
            long count = 0;
            _ArticleStructureService = new ArticleStructureService();
            return _ArticleStructureService.GetAllArticleStructure(categoryId: model.categoryId,
                                                                        count: ref count,
                                                                        size: model.pageSize,
                                                                        skip: model.pageIndex).GetHttpResponseWithCount(count);


        }


        [HttpPost]
        public HttpResponseMessage GetTreeArticleStructure(ArticleStructureSearchModel model)
        {
            _ArticleStructureService = new ArticleStructureService();
            long count = 0;
            return _ArticleStructureService.GetTreeArticleStructure(categoryId: model.categoryId,
                                                                        count: ref count,
                                                                        size: model.pageSize,
                                                                        skip: model.pageIndex).GetHttpResponseWithCount(count);

        }


        [HttpPost]
        public HttpResponseMessage GetAllArticleStructureParentForEdit(ArticleStructureParentEditModel model)
        {
            _ArticleStructureService = new ArticleStructureService();
            long count = 0;
            return _ArticleStructureService.GetAllArticleStructureForParentEdit(categoryId: model.categoryId,
                                                                               ArticleStructureId: model.articleStructureId,
                                                                               count: ref count,
                                                                               size: model.pageSize,
                                                                               skip: model.pageIndex).GetHttpResponseWithCount(count);


        }


        [HttpPost]
        public HttpResponseMessage ArticleStructureDetail(ArticleStructureFindModel model)
        {
            _ArticleStructureService = new ArticleStructureService();
            return _ArticleStructureService.GetSingleArticleStructure(model.articleStructureId).GetHttpResponse();
        }


        [HttpPost]
        public HttpResponseMessage ListArticle(ArticleFindModel model)
        {
            _ArticleService = new ArticleService();
            long count = 0;
            return _ArticleService.GetALLArticle(articleStructureId: model.articleStructureId,
                                                  count: ref count,
                                                  size: model.pageSize,
                                                  skip: model.pageIndex).GetHttpResponseWithCount(count);

        }


        #endregion


        #region Edit

        [HttpPost]
        public HttpResponseMessage EditArticleStructure(EditArticleStructureModel model)
        {
            _ArticleStructureService = new ArticleStructureService();

            return _ArticleStructureService.EditArticleStructure(ArticleStructureId: model.articleStructureId,
                                                               categoryId: model.categoryId,
                                                               title: model.title,
                                                               parentId: model.parentArticleStructureId,
                                                               logoUrl: model.logoUrl).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage EditArticle(EditArticleModel model)
        {
            _ArticleService = new ArticleService();
            if (model.featureValueUpdateStatus)
            {

                List<ArticleStructureFeature> featureValue = new List<ArticleStructureFeature>();

                foreach (var item in model.selectedItems)
                {

                    featureValue.Add(new ArticleStructureFeature
                    {
                        CategoryFeatureId = item.categoryFeatureId,
                        CustomValue = item.customValue,
                        FeatureValueId = item.featureValueIds
                    });

                }

                return _ArticleService.EditArticle(title: model.title,
                                                 articleId: model.articleId,
                                                 articleStructureId: model.articleStructureId,
                                                 body: model.htmlContent,
                                                 brief: model.briefDescription,
                                                 logoUrl: model.logoUrl,
                                                 articleItem: featureValue).GetHttpResponse();
            }
            else
            {
                return _ArticleService.EditArticle(title: model.title,
                                                 articleId: model.articleId,
                                                 articleStructureId: model.articleStructureId,
                                                 body: model.htmlContent,
                                                 logoUrl: model.logoUrl,
                                                 brief: model.briefDescription).GetHttpResponse();
            }

        }

        [HttpPost]
        public HttpResponseMessage AdminInitEditArticle(FindSingleArticleModel model)
        {
            _ArticleService = new ArticleService();

            return _ArticleService.InitEditArticle(articleId: model.articleId).GetHttpResponse();
        }


        #endregion


        #region delete

        [HttpPost]
        public HttpResponseMessage DeleteArticleStructure(ArticleStructureFindModel model)
        {
            _ArticleStructureService = new ArticleStructureService();
            return _ArticleStructureService.DeleteArticleStructure(ArticleStructureId: model.articleStructureId).GetHttpResponse();


        }


        [HttpPost]
        public HttpResponseMessage RemoveArticle(FindSingleArticleModel model)
        {
            _ArticleService = new ArticleService();
            return _ArticleService.RemoveArticle(articleId: model.articleId).GetHttpResponse();

        }

        #endregion

        #endregion
    }
}
