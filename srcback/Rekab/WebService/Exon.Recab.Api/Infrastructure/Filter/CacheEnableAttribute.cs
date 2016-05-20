using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;

namespace Exon.Recab.Api.Infrastructure.Filter
{
    [AttributeUsage(AttributeTargets.All)]
    public class CacheEnableAttribute : ActionFilterAttribute
    {
        private static IDatabase _rdb;

        private TimeSpan Time;

        static CacheEnableAttribute()
        {
            var settingsReader = new AppSettingsReader();

            var value = settingsReader.GetValue("WebApiRedisConnectionString", typeof(string));
            string connectionstring = value as string;

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionstring);

            _rdb = redis.GetDatabase();
        }

        public CacheEnableAttribute(string time)
        {
            string[] clock = time.Split(':');
            this.Time = new TimeSpan(hours: Convert.ToInt32(clock[0]),
                                     minutes: Convert.ToInt32(clock[1]),
                                     seconds: Convert.ToInt32(clock[2])
                                    );
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            JavaScriptSerializer scr = new JavaScriptSerializer();

            string url = scr.Serialize(actionContext.Request.RequestUri);

            string content = scr.Serialize(actionContext.ActionArguments);


            string reuest = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(url + content));

            string message = "";

            message = _rdb.StringGet(reuest);

            if (message != null && message != "")
            {
                CasheSaveObject result = scr.Deserialize<CasheSaveObject>(message);

                actionContext.Response = new HttpResponseMessage
                {
                    Content = new ObjectContent(typeof(object), result.content, new JsonMediaTypeFormatter()),
                    StatusCode = result.status
                };
            }

        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                JavaScriptSerializer scr = new JavaScriptSerializer();

                string url = scr.Serialize(actionExecutedContext.Request.RequestUri);

                string content = scr.Serialize(actionExecutedContext.ActionContext.ActionArguments);

                string reuest = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(url + content));

                _rdb.StringSet(key: reuest,
                                   value: scr.Serialize(new CasheSaveObject
                                   {
                                       status = actionExecutedContext.Response.StatusCode,
                                       content = ((ObjectContent)actionExecutedContext.Response.Content).Value
                                   }),
                                   when: When.NotExists,
                                   expiry: Time);


            });

        }

    }

    public class CasheSaveObject
    {
        public object content { get; set; }

        public HttpStatusCode status { get; set; }
    }
}
