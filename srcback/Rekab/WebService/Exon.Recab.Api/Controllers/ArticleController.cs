using Exon.Recab.Api.Models.Article;
using Exon.Recab.Service.Implement.ArTicl;
using System.Net.Http;
using System.Web.Http;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.ArtTicl;
using Exon.Recab.Api.Infrastructure.Filter;

namespace Exon.Recab.Api.Controllers
{
    public class ArticleController : ApiController
    {
        public readonly ArticleService _ArticleService;

        public readonly ArticleStructureService _ArticleStructureService;

        public ArticleController()
        {
            _ArticleService = new ArticleService();
            _ArticleStructureService = new ArticleStructureService();
        }


        #region   search

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage SearchArticleStructure(ArticleStructureSearchModel model)
        {
            long count = 0;
            return _ArticleStructureService.SearchArticleStructureByParent(categoryId: model.categoryId,
                                                                        parentId: model.parentArticleStructure,
                                                                        count: ref count,
                                                                        size: model.pageSize,
                                                                        skip: model.pageIndex).GetHttpResponseWithCount(count);


        }


        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage GetAllArticleStructure(ArticleStructureSearchModel model)
        {
            long count = 0;
            return _ArticleStructureService.GetAllArticleStructure(categoryId: model.categoryId,
                                                                        count: ref count,
                                                                        size: model.pageSize,
                                                                        skip: model.pageIndex).GetHttpResponseWithCount(count);


        }


        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage GetTreeArticleStructure(ArticleStructureSearchModel model)
        {
            long count = 0;
            return _ArticleStructureService.GetTreeArticleStructure(categoryId: model.categoryId,
                                                                        count: ref count,
                                                                        size: model.pageSize,
                                                                        skip: model.pageIndex).GetHttpResponseWithCount(count);

        }


        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage GetAllArticleStructureParentForEdit(ArticleStructureParentEditModel model)
        {
            long count = 0;
            return _ArticleStructureService.GetAllArticleStructureForParentEdit(categoryId: model.categoryId,
                                                                               ArticleStructureId: model.articleStructureId,
                                                                               count: ref count,
                                                                               size: model.pageSize,
                                                                               skip: model.pageIndex).GetHttpResponseWithCount(count);


        }


        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage ArticleStructureDetail(ArticleStructureFindModel model)
        {
            return _ArticleStructureService.GetSingleArticleStructure(model.articleStructureId).GetHttpResponse();
        }


        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage ListArticle(ArticleFindModel model)
        {
            long count = 0;
            return _ArticleService.GetALLArticle(articleStructureId: model.articleStructureId,
                                                  count: ref count,
                                                  size: model.pageSize,
                                                  skip: model.pageIndex).GetHttpResponseWithCount(count);

        }


        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage ArticleSearch(ArticleSearchModel model)
        {
            long count = 0;
            return _ArticleService.ArticleSearch(articleStructureId: model.articleStructureId,
                                                 keyword: model.keyword != null ? (model.keyword.Length >= 2 ? model.keyword : "") : "",
                                                 categoryId: model.categoryId,
                                                  count: ref count,
                                                  size: model.pageSize,
                                                  skip: model.pageIndex).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage ArticleDetail(FindSingleArticleModel model)
        {
            return _ArticleService.GetSingleArticle(articleId: model.articleId, userId: model.userId).GetHttpResponse();

        }


        public HttpResponseMessage AddRate(ArticleAddRateModel model)
        {

            return _ArticleService.AddRate(userId : model.userId, articleId : model.articleId , rate : model.rate).GetHttpResponse();
        }

        #endregion

    }
}
