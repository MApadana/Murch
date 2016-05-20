using Exon.Recab.Api.Infrastructure.Filter;
using Exon.Recab.Infrastructure.Exception;
using System.Linq;
using System.Web.Http;

namespace Exon.Recab.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            RecabException re = new RecabException();
            CacheEnableAttribute ne = new CacheEnableAttribute("0:0:0");         
            
            // Web API routes
            config.MapHttpAttributeRoutes();
            //config.EnableCors();

           config.Filters.Add(new RecabExceptionAttribute());
           config.Filters.Add(new RecabActionFilterAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
          name: "MobApi",
          routeTemplate: "mobapi/{controller}/{action}/{id}",
          defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional }
      );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
