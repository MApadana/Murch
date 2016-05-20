using Exon.Recab.Authentication.Content.Models;
using Exon.Recab.Authentication.Content.Resources;
using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Exon.Recab.Authentication.Content.Service
{
    public class RequestService
    {

        private long TimeLimit;

        private readonly string ApiUrl;

        private readonly string UploadUrl;

        private static int RequestNumber = 0;

        private bool TokenState;

        private static List<ResponseModel> ResponseList;

        public RequestService(string apiUrl, string uploadUrl, long timeLimit)
        {
            this.TimeLimit = timeLimit;
            this.UploadUrl = uploadUrl;
            this.ApiUrl = apiUrl;

        }

        public List<ResponseModel> InitRequest(List<MultiRequestModel> requestList)
        {
            RequestNumber = 0;
            ResponseList = new List<ResponseModel>();

            RecabException RecabException = new RecabException();
            RecabException.IsMobile = true;

            foreach (var item in requestList)
            {
                System.Threading.Thread newTheread = new System.Threading.Thread(GetResponse);
                newTheread.Start(item);
            }

            while (RequestNumber < requestList.Count()) { }

            JavaScriptSerializer scr = new JavaScriptSerializer();

            foreach (var item in ResponseList)
            {
                int exseptionCode = Convert.ToInt32(scr.Deserialize<Dictionary<string, object>>(item.result.ToString())["exceptionCode"]);

                if (exseptionCode == 6002 && !TokenState)
                {
                 
                    RecabException.StatusCode = HttpStatusCode.Unauthorized;

                    throw RecabException;
                }
            }

            return ResponseList.OrderBy(r => r.orderId).ToList();
        }

        public void GetResponse(object obj)
        {

            MultiRequestModel model = (MultiRequestModel)obj;

            var urlParametr = model.url.ToLower().Split('/');

            HttpWebRequest httpWebRequest;

            switch (urlParametr[0])
            {
                case "content":
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(UploadUrl + model.url);
                    break;
                default:
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(ApiUrl + model.url);
                    break;
            }



            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            RecabException RecabException = new RecabException();
            RecabException.IsMobile = true;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(TokenVerify(model.data, model.auth, model.url, TimeLimit, out TokenState, RecabException));
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
                ResponseList.Add(new ResponseModel
                {
                    result = e.Message,
                    orderId = model.orderId
                });
                RequestNumber++;
                return;

            }

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                ResponseList.Add(new ResponseModel
                {
                    result = result,
                    orderId = model.orderId
                });

            }
            RequestNumber++;
        }

        public static string TokenVerify(string data, string auth, string url, long TimeLimit, out bool tokenVerify ,RecabException RecabException)
        {
            JavaScriptSerializer scr = new JavaScriptSerializer();

            SdbContext db = new SdbContext();

           string tokenstring = auth;

           // db.Users.ToList();
            Domain.Entity.UserToken token = db.UserTokens.FirstOrDefault(c => c.Token == tokenstring);

            DateTime Time = DateTime.UtcNow.AddMinutes(-TimeLimit);


            var urlParametr = url.ToLower().Split('/');

            switch (urlParametr[0])
            {
                case "content":

                    if (token == null || token.InsertTime < Time || data == null)
                    {
                        RecabException.StatusCode = HttpStatusCode.Unauthorized;
                        throw RecabException;
                    }
                    break;
                default:
                    break;
            }


            if (token != null && token.InsertTime > Time)
            {
                if (!token.Available)
                {
                    RecabException.StatusCode = HttpStatusCode.Unauthorized;
                    throw RecabException;
                }

                tokenVerify = true;
                StringBuilder stb = new StringBuilder(data);

                if (data == null || data == "" || data.Length <= 2)
                {
                    data = "{}";

                    data = data.Insert(1, " \"userId\":" + token.UserId.ToString());
                }
                else
                {
                    switch (token.TokenType)
                    {
                        case TokenType.General:
                            data = data.Insert(1, " \"userId\":" + token.UserId.ToString() + ",");
                            break;

                        case TokenType.ForgetPassword:
                            if (urlParametr[2].ToLower() != "forgetpassword")
                                throw new RecabException(HttpStatusCode.Unauthorized);

                            data = data.Insert(1, " \"forgetUserId\":" + token.UserId.ToString() + ",");
                            break;
                    }

                }

                token.LastUsedTime = DateTime.UtcNow;
                db.SaveChanges();

            }
            else
            {
                tokenVerify = false;
                StringBuilder stb = new StringBuilder(data);
                if (data == null || data == "" || data.Length <= 2)
                {
                    data = "{}";

                    data = data.Insert(1, " \"userId\": 0");
                }
                else
                {
                    data = data.Insert(1, " \"userId\":0,");
                }

            }


            object obj = data != null ? scr.Deserialize<object>(data) : new object();

            //check acl                    
            return scr.Serialize(obj); ;

        }

        public static void VerifyModelValues(SingleRequestModel model)
        {
            if (model == null)
                return;

            var badRequestKeywords = BadRequestSignatures.badRequestKeywords;

            foreach (var key in badRequestKeywords)
                if (model.auth.Contains(key) || model.data.Contains(key) || model.data.Contains(key))
                    throw new RecabException("Some danger items has been detected in your request body.", HttpStatusCode.BadRequest);
        }

    }
}
