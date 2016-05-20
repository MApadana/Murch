using Exon.Recab.Api.Models.News;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.News;
using System.Net.Http;
using System.Web.Http;

namespace Exon.Recab.Api.Controllers
{
    public class NewsController : ApiController
    {
        private NewsService _newsService;

        public NewsController()
        {
            _newsService = new NewsService();
        }

        [HttpPost]
        public HttpResponseMessage AddEmailForSubscribe(AddEmailSubscribeModel model)
        {
            return _newsService.AddEmailToNewsList(email: model.email).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddNews(AddSubscribeModel model)
        {
            return _newsService.AddNews(title:model.title , brif : model.brife , body:model.body, type: (int)model.type).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage EditNews(EditSubscribeModel model)
        {
            return _newsService.EditNews(id:model.id,title: model.title, brif: model.brife, body: model.body, type: (int)model.type).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ListNews(ListNewsModel model)
        {
            long count = 0; 
            return _newsService.GetAllNews(type:(int)model.type, count :  ref count ,size : model.pageSize , skip : model.pageIndex).GetHttpResponseWithCount(count);
        }


        [HttpPost]
        public HttpResponseMessage SendNews(SendNewsModel model)
        {

             return _newsService.SendNews(id : model.id , roleId:model.roleId).GetHttpResponse();
        }

    }
}
