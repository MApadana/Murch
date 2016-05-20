using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.UserModel
{
    public class DelershipAdminViewModel
    {
        public long dealearshipId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string address { get; set; }

        public string fax { get; set; }

        public string tell { get; set; }

        public string logoUrl { get; set; }

        public int status { get; set; }

        public string confirmDate { get; set; }

        public string insertDate { get; set; }
        public string titleStatus { get;  set; }
    }
}
