
namespace Exon.Recab.Service.Model.UserModel
{
    public class DelershipViewModel
    {
        public long id { get; set; }

        public string address { get;  set; }

        public string description { get;  set; }

        public string fax { get;  set; }

        public double lat { get;  set; }

        public double lng { get;  set; }

        public string logoUrl { get;  set; }

        public string tell { get;  set; }

        public string title { get;  set; }

        public long[] categoryId { get;set; }
    }
}
