using Exon.Recab.Service.Implement.PolicySystemConfig;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Net.Http;
using System.Web.Http;
using Exon.Recab.Api.Infrastructure.Filter;

namespace Exon.Recab.Api.Controllers
{
    public class SystemConfigController : ApiController
    {
        public readonly ProductPolicyService _PolicyService;

        public SystemConfigController()
        {
            _PolicyService = new ProductPolicyService();
        }

        [HttpPost]
        [CacheEnable("100:0:0")]
        public HttpResponseMessage GetADPicture()
        {
            return new { data = _PolicyService.GetAddefaultPictureCount() }.GetHttpResponse();
        }
    }
}
