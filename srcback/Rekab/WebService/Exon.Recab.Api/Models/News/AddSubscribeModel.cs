using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.News
{
    public class AddSubscribeModel
    {
        public string body { get; set; }
        public string brife { get; set; }
        public string title { get;  set; }
        public NewsType type { get;  set; }
    }

    public enum NewsType {

        پیام_کوتاه=0,
        ایمیل=1
    }
}