
using Exon.Recab.Authentication.Content.Infrastructure;
using System.Web.Http;

namespace Exon.Recab.Authentication.Content
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var ex = new RecabExceptionAttribute();

            config.MapHttpAttributeRoutes();

            config.Filters.Add(new RecabExceptionAttribute());

            //config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "Routing/V1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }


}
