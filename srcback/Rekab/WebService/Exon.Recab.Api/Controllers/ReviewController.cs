using Exon.Recab.Api.Models.Review;
using Exon.Recab.Service.Implement.ReView;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Model.ReviewModel;
using System.Linq;
using Exon.Recab.Api.Infrastructure.Filter;

namespace Exon.Recab.Api.Controllers
{
    public class ReviewController : ApiController
    {
        #region init
        public readonly ReviewService _ReviewService;

        public ReviewController()
        {
            _ReviewService = new ReviewService();
        } 
        #endregion

        #region report

        [HttpPost]
        public HttpResponseMessage ReviewDetail(FindSingleReviewModel model)
        {
            return _ReviewService.GetSingleReview(reviewId: model.reviewId , userId : model.userId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ReviewCompare(CompareReviewModel model)
        {
            List<ReviewViewModel> result = new List<ReviewViewModel>();

            List<long> CFId = new List<long>();
            foreach (var item in model.reviewIds)
            {
                ReviewViewModel newReview = _ReviewService.GetSingleReview(reviewId: item, userId: model.userId);

                result.Add(newReview);

                foreach (var cf in newReview.reviewCategoryFeatures.Select(f=>f.categoryFeatureId))
                {
                    if (!CFId.Any(i => i == cf))
                        CFId.Add(cf);

                }
            }

            foreach (var item in result)
            {
                foreach (var cf in CFId)
                {
                    if (!item.reviewCategoryFeatures.Any(ggg => ggg.categoryFeatureId == cf))
                    {

                        item.reviewCategoryFeatures.Add(new ReviewCategoryFeatureViewModel { categoryFeatureId = cf, featureValues = new List<ReviewFeatureValueViewModel>(), title = "" });
                    }
                }
            }
            return result.GetHttpResponse();
        }
        
        [HttpPost]
        [CacheEnable("100:0:0")]
        public HttpResponseMessage ReviewSearch(ReviewSearchModel model)
        {
            List<ReviewFilterModel> filter = new List<ReviewFilterModel>();

            foreach (var item in model.selectedItems)
            {
                filter.Add(new ReviewFilterModel { CategoryFeatureId = item.categoryFeatureId, FeatureValueId = (item.featureValueIds.Count > 0 ? item.featureValueIds.FirstOrDefault() : new long()) });
            }
            long count = 0;
            return _ReviewService.ReviewSearch(categoryId: model.categoryId,
                 keyword: model.keyword != null ? (model.keyword.Length >= 2 ? model.keyword : "") : "",
                filter: filter,
                count: ref count,
                skip: model.pageIndex,
                size: model.pageSize).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage ReviewGroupBy(ReviewSearchModel model)
        {
            return _ReviewService.ReviewSearchLogo(categoryId: model.categoryId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddRate(AddReviewRateModel model)
        {
       
            return _ReviewService.AddRate(userId: model.userId, reviewId: model.reviewId, rate: model.rate).GetHttpResponse();
        }

        #endregion

    }
}
