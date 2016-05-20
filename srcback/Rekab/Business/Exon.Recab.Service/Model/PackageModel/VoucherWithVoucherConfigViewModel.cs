using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PackageModel
{
    public class VoucherWithVoucherConfigViewModel
    {
        public string fromPersianDate { get; set; }
        public string responseCode { get; set; }
        public string toPersianDate { get; set; }
        public long userId { get; set; }
        public object username { get; set; }
        public string voucherCode { get; set; }
        public string voucherConfigTitle { get; set; }
    }
}
