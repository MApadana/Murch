using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PackageModel
{
    public class VoucherConfigViewModel
    {
        public string type { get; set; }

        public long count { get;  set; }
        public string creatDate { get;  set; }
        public string description { get;  set; }
        public string fromDate { get;  set; }
        public string title { get;  set; }
        public string toDate { get;  set; }
        public string userCreator { get;  set; }
        public long voucherConfigId { get;  set; }
        public string value { get; set; }
    }
}
