using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exon.Recab.Domain.Constant.User;

namespace Exon.Recab.Service.Model.UserModel
{
    public class DealershipEditViewModel
    {
        public string address { get;  set; }

        public List<long> categoryitems { get;  set; }

        public long dealershipId { get;  set; }

        public double lat { get;  set; }

        public double lng { get;  set; }

        public string tell { get;  set; }

        public string title { get;  set; }

        public string fax { get;  set; }

        public string description { get;  set; }

        public long stateId { get;  set; }

        public string stateTitle { get;  set; }

        public string cityTitle { get;  set; }

        public long cityId { get;  set; }

        public string logoUrl { get;  set; }

        public string websiteUrl { get;  set; }

        public long cumUserId { get; set; }

        public DealershipStatus status { get;  set; }

        public DealershipEditViewModel()
        {
            categoryitems = new List<long>();
        }
    }
}
