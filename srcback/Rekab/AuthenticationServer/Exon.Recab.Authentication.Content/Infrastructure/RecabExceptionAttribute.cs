using System;
using System.Web.Http.Filters;
using System.Net;
using System.Diagnostics;
using System.Web.Mvc;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Exon.Recab.Authentication.Content.Infrastructure
{
    [AttributeUsage(AttributeTargets.All)]

    public class RecabExceptionAttribute : ExceptionFilterAttribute
    {

        private readonly Action<HttpActionExecutedContext> _actionOnException;

        public RecabExceptionAttribute() : this(delegate (HttpActionExecutedContext simpleExseption)
        {



            JavaScriptSerializer scr = new JavaScriptSerializer();

            var a = "".GetHttpResponseError(status: HttpStatusCode.InternalServerError).Content as ObjectContent;

            object temp = new { result = scr.Serialize("".GetHttpResponseError(status: HttpStatusCode.InternalServerError)) };

            simpleExseption.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new ObjectContent(typeof(object), temp, new JsonMediaTypeFormatter()) };
        })
        { }

        public RecabExceptionAttribute(Action<HttpActionExecutedContext> actionOnException)
        {
            this._actionOnException = actionOnException;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is RecabException)
            {
                RecabException Recab = actionExecutedContext.Exception as RecabException;


                if (!Recab.IsMobile)
                {
                    JavaScriptSerializer scr = new JavaScriptSerializer();

                    var a = Recab.RMessage.GetHttpResponseError(status: Recab.StatusCode, type: (int)Recab.Type).Content as ObjectContent;
                   
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK,
                                                                                Content = new ObjectContent(typeof(object),
                                                                                                            scr.Deserialize<object>(scr.Serialize(a.Value)),
                                                                                                            new JsonMediaTypeFormatter())
                                                                              };
                }
                else {
                    JavaScriptSerializer scr = new JavaScriptSerializer();

                    var a = Recab.RMessage.GetHttpResponseError(status: Recab.StatusCode, type: 2001).Content as ObjectContent;

                    object temp = new { result = scr.Serialize(a.Value) };                                      

                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK,
                                                                                Content = new ObjectContent(typeof(object),
                                                                                                            temp,
                                                                                                            new JsonMediaTypeFormatter())
                                                                               };
                }
            }
            else
            {
                this._actionOnException(actionExecutedContext);
            }


            Debug.WriteLine(actionExecutedContext.Exception.Message);
        }

    }

}