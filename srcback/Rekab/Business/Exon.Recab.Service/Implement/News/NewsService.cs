using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.News;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Exon.Recab.Service.Implement.News
{
    public class NewsService
    {
        private readonly MdbContext _mdb;
        private readonly SdbContext _sdb;

        public NewsService()
        {
            _mdb = new MdbContext();
            _sdb = new SdbContext();
        }

        public bool AddEmailToNewsList(string email)
        {

            if (_mdb.UserNews.Find(filter: new BsonDocument { { "Email", email } }).Count() > 0)
                throw new RecabException((int)ExceptionType.RedundantUserNewsEmail);

            _mdb.UserNews.InsertOne(new BsonDocument { { "Email", email },
                                                       { "CreateDate" , DateTime.UtcNow.ToString()  } });

            return true;

        }


        public bool AddNews(string title, string brif, string body, int type)
        {
            _mdb.News.InsertOne(new BsonDocument { { "Title",title },
                                                   { "Brif",brif },
                                                   { "Body",body },
                                                   { "Type",(int)type},
                                                   { "NewsSendType",(int)NewsSendType.NotSend }
                                                  });
            return true;
        }


        public bool EditNews(string id ,string title, string brif, string body, int type)
        {

            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));

            var update = Builders<BsonDocument>.Update
                .Set("Title", title)
                .Set("Brif", brif)
                .Set("Body", body)
                .Set("Type", (int)type)                
                .CurrentDate("lastModified");

            _mdb.News.UpdateMany(filter, update);

            return true;
        }

        public List<object> GetAllNews(int type, ref long count, int size = 1, int skip = 0)
        {
            count = _mdb.News.Count(new BsonDocument { { "Type", type } });

                        
            List<BsonDocument> MongoResultresult = _mdb.News.Find(new BsonDocument { { "Type", type } }).Skip(size * skip).Limit(size).ToList();

            List<object> resullt = new List<object>();

            foreach (var item in MongoResultresult)
            {
                resullt.Add(BsonSerializer.Deserialize<object>(item));
            }
            
            return  resullt;

        }

        public bool SendNews(string id, long? roleId)
        {
            BsonDocument news = _mdb.News.Find(new BsonDocument { { "_id", ObjectId.Parse(id) } }).FirstOrDefault();

            if (news.ElementCount == 0)
                throw new RecabException((int)ExceptionType.NewsNotFound);
            if (roleId.HasValue)
            {
                if (!_sdb.Roles.Any(r => r.Id == roleId.Value))
                    throw new RecabException((int)ExceptionType.RoleNotFound);

            }

            switch ((NewsType)Convert.ToInt32(news.GetElement("Type").Value.ToString()))
            {
                case NewsType.Email:

                    object obj = new object();

                    obj = obj.AddNewField("url", "Subscribe");

                    obj = obj.AddNewField("data", (new { id = id, type = 1 }).ToJsonString());

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.14:7578");

                    httpWebRequest.ContentType = "text/json";

                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(obj.ToJsonString());
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
                        throw new RecabException(e.Message);

                    }

                    break;
                case NewsType.SMS:

                    object smsobj = new object();

                    smsobj = smsobj.AddNewField("url", "Subscribe");

                    smsobj = smsobj.AddNewField("data", (new { id = id, type = 0 }).ToJsonString());

                    HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://192.168.1.14:7578");

                    Request.ContentType = "text/json";

                    Request.Method = "POST";

                    using (var streamWriter = new StreamWriter(Request.GetRequestStream()))
                    {
                        streamWriter.Write(smsobj.ToJsonString());
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse Response;
                    try
                    {
                        Response = (HttpWebResponse)Request.GetResponse();
                    }
                    catch (Exception e)
                    {
                        throw new RecabException(e.Message);

                    }

                    break;



            }


            return true;
        }

    }
}
