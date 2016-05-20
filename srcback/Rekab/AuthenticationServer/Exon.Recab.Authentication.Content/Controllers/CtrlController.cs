using Exon.Recab.Authentication.Content.Service;
using Exon.Recab.Authentication.Content.Models;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Script.Serialization;
using MsgPack.Serialization;
using System.Collections.Generic;

namespace Exon.Recab.Authentication.Content.Controllers
{
    public class CtrlController : ApiController
    {
        private readonly long TimeLimit;

        private readonly string ApiUrl;

        private readonly string UploadUrl;

        private readonly bool MessagePackEnable;

        private bool TokenStatus;

        public CtrlController()
        {
            var settingsReader = new AppSettingsReader();

            var value = settingsReader.GetValue("TokenTimeOut", typeof(string));
            this.TimeLimit = long.Parse(value as string);

            value = settingsReader.GetValue("AplicationApiUrl", typeof(string));
            this.ApiUrl = value as string;

            value = settingsReader.GetValue("AplicationUploadUrl", typeof(string));
            this.UploadUrl = value as string;

            value = settingsReader.GetValue("MessagePackEnable", typeof(string));
            MessagePackEnable = Convert.ToBoolean(value as string);

        }


        [HttpPost]
        public HttpResponseMessage Post(InputModel sampleModel)
        {

            JavaScriptSerializer scr = new JavaScriptSerializer();

            scr.MaxJsonLength = int.MaxValue;

            SingleRequestModel model = scr.Deserialize<SingleRequestModel>(sampleModel.data);

            RecabException RecabException = new RecabException();
            if (model.url.Split('/')[0].ToLower() == "api" || model.url.Split('/')[0].ToLower() == "apicontent")
            {
                RecabException = new RecabException { IsMobile = false };
            }
            else
            {
                RecabException = new RecabException { IsMobile = true };
            }


            var urlParametr = model.url.ToLower().Split('/');

            HttpWebRequest httpWebRequest;

            switch (urlParametr[0])
            {
                case "content":
                case "apicontent":
                    RequestService.VerifyModelValues(model);
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(UploadUrl + model.url.ToLower().Replace("api", ""));
                    break;
                //case "mobapi":
                //    RequestService.VerifyModelValues(model);
                //    httpWebRequest = (HttpWebRequest)WebRequest.Create(MobileApiUrl + model.url.ToLower());
                //    break;

                default:
                    RequestService.VerifyModelValues(model);
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(ApiUrl + model.url);
                    break;
            }

            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(RequestService.TokenVerify(model.data, model.auth, model.url, TimeLimit, out TokenStatus ,RecabException : RecabException));
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                RecabException.RMessage = e.Message;
                RecabException.StatusCode = HttpStatusCode.BadRequest;

                throw RecabException;
            }

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {

                var result = streamReader.ReadToEnd();

                var serializer = SerializationContext.Default.GetSerializer<object>();

                if (MessagePackEnable)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {

                        // Pack obj to stream.
                        serializer.Pack(ms, scr.Deserialize<object>(result));
                        ms.Flush();
                        ms.Close();

                        result = Convert.ToBase64String(ms.ToArray());
                    }
                    System.GC.Collect();
                }

                int exseptionCode = Convert.ToInt32(scr.Deserialize<Dictionary<string, object>>(result)["exceptionCode"]);
                if (exseptionCode == 6002 && !TokenStatus)
                {
                    
                    RecabException.StatusCode = HttpStatusCode.Unauthorized;

                    throw RecabException;
                }



                if (model.url.Split('/')[0].ToLower() == "api" || model.url.Split('/')[0].ToLower() == "apicontent")
                {

                    return new HttpResponseMessage
                    {
                        Content = new ObjectContent(typeof(object), scr.Deserialize<object> (result), new JsonMediaTypeFormatter()),
                        StatusCode = HttpStatusCode.OK
                    };
                }
                return new HttpResponseMessage
                {
                    Content = new ObjectContent(typeof(object), new { result = result }, new JsonMediaTypeFormatter()),
                    StatusCode = httpResponse.StatusCode
                };

            }

            throw RecabException;

        }

        //[HttpPut]
        //public string Put(SingleRequestModel data)
        //{
        //    return "";
        //}

    }
}
