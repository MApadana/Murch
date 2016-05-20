using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.User
{
    public class UserEditDealershipModel
    {
        public long userId { get; set; }

        public string address { get; set; }
        public List<long> categoryIds { get; set; }
        public long cityId { get; set; }
        public long dealershipId { get; set; }
        public string description { get; set; }
        public string fax { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string logoUrl { get; set; }
        public string tell { get; set; }
        public string title { get; set; }

        public string websiteUrl { get; set; }

        public UserEditDealershipModel()
        {
            categoryIds = new List<long>();
        }
    }
}
