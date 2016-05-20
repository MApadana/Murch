using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.User
{
    public class BuyPackageModel
    {
        public long userId { get; set; }

        public long cpptId { get;  set; }
        
        public string voucherCode { get; set; }
    }
}
