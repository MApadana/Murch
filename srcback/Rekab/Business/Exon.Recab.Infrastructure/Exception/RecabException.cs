using System.Net;
using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.SqlServer;
using StackExchange.Redis;
using System.Configuration;
using System.Linq;
using Exon.Recab.Domain.Entity.ExceptionModule;
using System.Collections.Generic;

namespace Exon.Recab.Infrastructure.Exception
{
    public class RecabException : System.Exception
    {

        private static IDatabase _rdb;

        static RecabException()
        {

            var settingsReader = new AppSettingsReader();

            var value = settingsReader.GetValue("InfrastructureRedisConnectionString", typeof(string));
            string connectionstring = value as string;



            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionstring);

            _rdb = redis.GetDatabase();

            string Message = _rdb.StringGet("ExseptionExist");

            if (!(Message == "True"))
            {
                SdbContext sdb = new SdbContext();

                List<ExceptionCode> ecode = sdb.ExceptionCodes.ToList();

                foreach (var item in ecode)
                {
                    _rdb.StringSet(key: item.ExceptionType.ToString(),
                                    value: item.Message,
                                    when: When.NotExists,
                                    expiry: System.TimeSpan.MaxValue);
                }

                _rdb.StringSet(key: "ExseptionExist",
                                    value: "True",
                                    when: When.Always,
                                    expiry: System.TimeSpan.MaxValue);
            }

        }

        public RecabException()
        {
            this.RMessage = _rdb.StringGet(ExceptionType.InternalError.ToString());
            this.RMessage = this.RMessage != "0" ? this.RMessage : "";
            this.StatusCode = HttpStatusCode.InternalServerError;
        }

        public RecabException(string message)
        {
            this.RMessage = message;
            this.StatusCode = HttpStatusCode.BadRequest;
            this.Type = (int)ExceptionType.InternalError;
        }

        public RecabException(HttpStatusCode code)
        {
            this.RMessage = "";
            this.StatusCode = code;
            this.Type = (int)ExceptionType.BadRequest;
        }

        public RecabException(string message, HttpStatusCode code)
        {
            this.RMessage = message;
            this.StatusCode = code;
            this.Type = (int)ExceptionType.BadRequest;
        }

        public RecabException(int type)
        {

            this.RMessage = _rdb.StringGet(((ExceptionType)type).ToString());
            this.RMessage = this.RMessage != "0" ? this.RMessage : "";
            this.StatusCode = HttpStatusCode.BadRequest;
            this.Type = (int)type;
        }

        public RecabException(int type, string message)
        {

            string data = _rdb.StringGet(((ExceptionType)type).ToString());

            this.RMessage = (data != null && data != "") ? string.Format(data, message) : "";

           this.StatusCode = HttpStatusCode.BadRequest;

            this.Type = (int)type;
        }

        public string RMessage { get; set; }

        public int Type { get; set; }

        public bool IsMobile { get; set; }

        public HttpStatusCode StatusCode { get; set; }

    }
}