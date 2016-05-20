using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace Exon.Recab.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
                  
            
        }

        protected void Application_ReleaseRequestState(object sender, EventArgs e)
        {

            if (Response.StatusCode != 200)
            {
                //Response.ClearContent();
                //Response.Clear();
                Response.StatusCode = 200;
            }
            
        }
    }
}
