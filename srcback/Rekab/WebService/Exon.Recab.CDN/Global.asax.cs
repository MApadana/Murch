using System.Web.Http;
using System.Web.Mvc;

namespace Exon.Recab.CDN
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
         
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
