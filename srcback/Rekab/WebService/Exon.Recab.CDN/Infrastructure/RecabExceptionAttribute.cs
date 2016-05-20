using System;
using System.Web.Http.Filters;
using System.Net;
using System.Diagnostics;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Web.Http.ExceptionHandling;

namespace Exon.Recab.CDN.Infrastructure                                                                                         
{
    
    [AttributeUsage(AttributeTargets.All)]
   
    public class RecabExceptionAttribute : ExceptionFilterAttribute
    {

        private readonly Action<HttpActionExecutedContext> _actionOnException;

        public RecabExceptionAttribute() : this(delegate (HttpActionExecutedContext simpleExseption)
        {
            simpleExseption.Response = "someting wrong".GetHttpResponse(HttpStatusCode.InternalServerError);
        })
        {
        }

        public RecabExceptionAttribute(Action<HttpActionExecutedContext> actionOnException)
        {
            this._actionOnException = actionOnException;
        }

        public void OnException(ExceptionContext filterContext)
        {
            throw new NotImplementedException();
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is RecabException)
            {
                RecabException Recab = actionExecutedContext.Exception as RecabException;

                actionExecutedContext.Response = Recab.RMessage.GetHttpResponseError(HttpStatusCode.BadRequest);
            }
            else
            {
                this._actionOnException(actionExecutedContext);
            }
            Debug.WriteLine(actionExecutedContext.Exception.Message);
        }

    }
}