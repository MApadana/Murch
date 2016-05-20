using Exon.Recab.Api.Infrastructure.Filter;
using Exon.Recab.Api.Models.BaseAdmin;
using Exon.Recab.Api.Models.Public;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.Admin;
using System.Net.Http;
using System.Web.Http;

namespace Exon.Recab.Api.Controllers
{
    public class PublicController : ApiController
    {
        public readonly AdminCategoryService _categoryService;

        public PublicController()
        {
            _categoryService = new AdminCategoryService();
        }

        [HttpPost]  
        [CacheEnable("100:0:0")]
        public HttpResponseMessage ListCategory(ListCategoryModel model)
        {
            long count = 0;
            return _categoryService.GetAllCategorisSimple(count: ref count ,
                                                          size:model.pageSize ,
                                                          skip:model.pageIndex)
                                                         .GetHttpResponseWithCount(count);
        }

        [HttpPost]
        [CacheEnable("100:0:0")]
        public HttpResponseMessage ListState()
        {
            return _categoryService.GetAllStat().GetHttpResponse();
        }

        [HttpPost]
        [CacheEnable("100:0:0")]
        public HttpResponseMessage ListCity(FindModel model)
        {
            return _categoryService.GetCityOfState(model.id).GetHttpResponse();
        }

        [HttpPost]
        [CacheEnable("0:30:0")]
        public HttpResponseMessage ListCategoryExchange(CategoryExchangeFindModel model)
        {
            int count = 0;
            return _categoryService.GetAllExchangeCategoryFromCategorySimple(model.categoryId, ref count, model.pageSize, model.pageIndex).GetHttpResponseWithCount(count);
        }
    }
}
