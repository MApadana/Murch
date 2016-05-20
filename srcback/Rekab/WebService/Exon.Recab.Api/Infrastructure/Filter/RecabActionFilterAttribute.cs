using System;
using System.Collections.Generic;
using System.Web.Http.Filters;
using Exon.Recab.Infrastructure.Utility.Extension;
using StackExchange.Redis;
using System.Configuration;
using Exon.Recab.Api.Infrastructure.Resource;
using System.Web.Http.Controllers;

namespace Exon.Recab.Api.Infrastructure.Filter
{
    [AttributeUsage(AttributeTargets.All)]
    public class RecabActionFilterAttribute : ActionFilterAttribute
    {
        private static IDatabase _rdb;
        static RecabActionFilterAttribute()
        {
            var settingsReader = new AppSettingsReader();

            var value = settingsReader.GetValue("InfrastructureRedisConnectionString", typeof(string));
            string connectionstring = value as string;

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionstring);

            _rdb = redis.GetDatabase();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {

                string message = _rdb.StringGet(ModelStateResource.ModelStateInvalid);

                List<object> errors = new List<object>();

                foreach (var item in actionContext.ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        if (error.Exception == null)
                        {
                            string key = item.Key.ToString().Replace("model.", "");
                            errors.Add(new { key = key, value = string.Format(_rdb.StringGet(error.ErrorMessage), key) });
                        }
                    }

                }

                actionContext.Response = errors.GetHttpResponseError(message: message);
            }
        }
    }
}
