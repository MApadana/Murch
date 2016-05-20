using Exon.Recab.CDN.Models;
using Exon.Recab.CDN.Service;
using System.Web.Configuration;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Net.Http;
using System.Web.Http;
using System.Web.Hosting;

namespace Exon.Recab.CDN.Controllers
{
    public class UploadController : ApiController
    {

        private readonly UploadService _uploadService;
        public UploadController()
        {
            _uploadService = new UploadService();
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostFile(HttpPostedFileModel model)
        {
           string  path = HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["RecabUpload"]);

            return _uploadService.SavePostedFile(model.data, model.userId, path).GetHttpResponse();
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Base64(Base64InputModel model )
        {
            string path = HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["RecabUpload"]);

            //string path = WebConfigurationManager.AppSettings["RecabUpload"];
            return _uploadService.SaveBase64(model.data, model.userId, model.extension, path , model.sizeSensitive , model.waterMark).GetHttpResponse();

           
        }
    }
}