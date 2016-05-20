using Exon.Recab.Authentication.Content.Models;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Configuration;
using Exon.Recab.Authentication.Content.Service;

namespace Exon.Recab.Authentication.Content.Controllers
{
    public class NCtrlController : ApiController
    {

        private readonly RequestService _requestService;

        private readonly bool MessagePackEnable;
        public NCtrlController()
        {

            var settingsReader = new AppSettingsReader();

            var value = settingsReader.GetValue("TokenTimeOut", typeof(string));
            long TimeLimit = long.Parse(value as string);

            value = settingsReader.GetValue("AplicationApiUrl", typeof(string));
            string ApiUrl = value as string;

            value = settingsReader.GetValue("AplicationUploadUrl", typeof(string));
            string UploadUrl = value as string;

            value = settingsReader.GetValue("MessagePackEnable", typeof(string));
            MessagePackEnable = Convert.ToBoolean(value as string);


            _requestService = new RequestService(timeLimit: TimeLimit,
                                                 uploadUrl: UploadUrl,
                                                 apiUrl: ApiUrl);

        }

        [HttpPost]
        public HttpResponseMessage Post(InputModel sampleModel)
        {
           
            JavaScriptSerializer scr = new JavaScriptSerializer();


            List<MultiRequestModel> model = scr.Deserialize<List<MultiRequestModel>>(sampleModel.data);

     
            var response = _requestService.InitRequest(model);

            string result = scr.Serialize(response);

            if (MessagePackEnable)
            {
                var serializer = SerializationContext.Default.GetSerializer<object>();

                using (MemoryStream ms = new MemoryStream())
                {
                    // Pack obj to stream.
                    serializer.Pack(ms, result);
                    ms.Flush();
                    ms.Close();

                    result = Convert.ToBase64String(ms.ToArray());

                }
                System.GC.Collect();
            }
            return new HttpResponseMessage
            {
                Content = new ObjectContent(typeof(object), new { result = result }, new JsonMediaTypeFormatter()),
                StatusCode = HttpStatusCode.OK
            };


        }

        //[HttpPut]
        //public List<ResponseModel> Put(List<MultiRequestModel> data)
        //{
        //    return new List<ResponseModel>();
        //}
    }
}
