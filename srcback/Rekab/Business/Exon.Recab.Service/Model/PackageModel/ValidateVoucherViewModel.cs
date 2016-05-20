using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PackageModel
{
    public class ValidateVoucherViewModel
    {
        public string type { get; set; }

        public bool validate { get; set; }

        public string fromDate { get; set; }

        public string toDate { get; set; }

        public string message { get; set; }

        public string amountWithoutDiscount { get;set; }

        public string amountWithDisCount { get;  set; }
    }
}
