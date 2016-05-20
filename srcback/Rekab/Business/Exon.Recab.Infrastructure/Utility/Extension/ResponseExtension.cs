using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Exon.Recab.Infrastructure.Model;
using Exon.Recab.Domain.Constant.CS.Exception;

namespace Exon.Recab.Infrastructure.Utility.Extension
{
    public static class ResponseExtension
    {
        public static HttpResponseMessage GetHttpResponse(this object obj,
                                                          HttpStatusCode status = HttpStatusCode.OK)
        {

            return new HttpResponseMessage
            {
                Content = new ObjectContent(typeof(object),
                                            new SimpleResponse
                                            {
                                                data = obj,
                                                message = "",
                                                status = (int)status,
                                                exceptionCode = 0,
                                                modelStateError = new List<object>()
                                            },
                                            new JsonMediaTypeFormatter()),

                StatusCode = status
            };


        }

        public static HttpResponseMessage GetHttpResponseWithCount(this object obj,
                                                                     long count, 
                                                                     HttpStatusCode status = HttpStatusCode.OK)
        {

            return new HttpResponseMessage
            {
                Content = new ObjectContent(typeof(object),
                                           new ResponseWithCount
                                           {
                                               data = obj,
                                               message = "",
                                               total = count,
                                               status = (int)status ,
                                               exceptionCode = 0,
                                               modelStateError = new List<object>()
                                           },
                                           new JsonMediaTypeFormatter()),

                StatusCode = status
            };




        }

        public static HttpResponseMessage GetHttpResponseError(this string obj,
                                                               HttpStatusCode status = HttpStatusCode.InternalServerError,
                                                               int type = (int)ExceptionType.InternalError)
        {

            return new HttpResponseMessage
            {
                Content = new ObjectContent(typeof(object),
                                           new SimpleResponse
                                           {
                                               data = new object(),
                                               message = obj,
                                               status = (int)status,
                                               exceptionCode = (int)type,
                                               modelStateError =new List<object>()
                                           },
                                           new JsonMediaTypeFormatter()),

                StatusCode =  status
            };


        }

        public static HttpResponseMessage GetHttpResponseError(this List<Object> obj,
                                                               string message)
        {

            return new HttpResponseMessage
            {
                Content = new ObjectContent(typeof(object),
                                           new SimpleResponse
                                           {
                                               data = new object(),
                                               message = message,
                                               status = (int)HttpStatusCode.BadRequest,
                                               exceptionCode = (int)ExceptionType.ModelStateInvalid,
                                               modelStateError = obj

                                           },
                                           new JsonMediaTypeFormatter()),

                StatusCode = HttpStatusCode.BadRequest
            };


        }

    }
}