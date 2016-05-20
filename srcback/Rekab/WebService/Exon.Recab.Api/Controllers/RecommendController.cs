using Exon.Recab.Api.Infrastructure.Filter;
using Exon.Recab.Api.Models.Recommend;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.Recommend;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.ReviewModel;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Exon.Recab.Api.Controllers
{
    public class RecommendController : ApiController
    {
      
        private readonly RecommendService _RecommendService;

        public RecommendController()
        {          
           
            _RecommendService = new RecommendService();
        }

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage GetRelativeADS(RecommendAdvertiseModel model)
        {
            List<CFProdoctFilterModel> filter = new List<CFProdoctFilterModel>();


            return _RecommendService.GetReletiveAds(caregoryId: model.categoryId,
                                                    entityId: model.entityId, 
                                                    entityType:(int)model.type,
                                                    size: model.pageSize,
                                                    skip: model.pageIndex);

        }
    
        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage GetRelativeReview(RecommendAdvertiseModel model)
        {

            List<ReviewFilterModel> filter = new List<ReviewFilterModel>();

            // long count = 0;
            //return _ReviewService.ReviewSearch(categoryId: model.categoryId,
            //    keyword:"",
            //    filter: filter,
            //    count: ref count,
            //    skip: model.page,
            //    size: model.size).GetHttpResponseWithCount(count);

            return new HttpResponseMessage();
        }

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage GetBriefSearch(GetBriefSearchModel model)
        {
            return _RecommendService.BriefSearch(model.key).GetHttpResponse();            
        }

        [HttpPost]
        public HttpResponseMessage AddFeatureValueTag(AddFeatureValueTagModel model)
        {
            return _RecommendService.AddTageForFeatureValue(featureValueId: model.featureValueId , tag:model.key ).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage EditFeatureValueTag(AddFeatureValueTagModel model)
        {
            return _RecommendService.EditTageForFeatureValue(featureValueId: model.featureValueId, tag: model.key).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage GetFeatureValueTag(GetFeatureValueTagModel model)
        {
            return _RecommendService.GetTageForFeatureValue(featureValueId:model.featureValueId).GetHttpResponse();
        }

    }
}
