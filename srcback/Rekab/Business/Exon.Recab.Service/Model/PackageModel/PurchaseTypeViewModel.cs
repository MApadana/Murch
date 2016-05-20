using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PackageModel
{
    public class PurchaseTypeViewModel
    {
        public bool isDealership { get;  set; }
        public bool isFree { get;  set; }
        public string logoUrl { get;  set; }
        public long purchaseTypeId { get;  set; }
        public string title { get;  set; }
    }
}
