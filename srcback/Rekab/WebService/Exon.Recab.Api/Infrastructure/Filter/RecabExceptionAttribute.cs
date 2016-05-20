using System;
using System.Web.Http.Filters;
using System.Net;
using System.Diagnostics;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;

namespace Exon.Recab.Api.Infrastructure.Filter
{
    
    [AttributeUsage(AttributeTargets.All)]
   
    public class RecabExceptionAttribute : ExceptionFilterAttribute
    {

        private readonly Action<HttpActionExecutedContext> _actionOnException;

        public RecabExceptionAttribute() : this(delegate (HttpActionExecutedContext simpleExseption)
        {
            simpleExseption.Response = "".GetHttpResponseError(status: HttpStatusCode.InternalServerError,
                                                               type: 2000);
        }){}

        public RecabExceptionAttribute(Action<HttpActionExecutedContext> actionOnException)
        {
            this._actionOnException = actionOnException;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is RecabException)
            {
                RecabException  Recab = actionExecutedContext.Exception as RecabException;

                actionExecutedContext.Response =Recab.RMessage.GetHttpResponseError(status:Recab.StatusCode,
                                                                                      type: (int)Recab.Type);
            }
            else
            {
                this._actionOnException(actionExecutedContext);
            }


            Debug.WriteLine(actionExecutedContext.Exception.Message);
        }

    }
}