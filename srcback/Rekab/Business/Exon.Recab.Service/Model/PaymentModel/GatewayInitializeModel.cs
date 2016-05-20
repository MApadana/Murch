using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exon.Recab.Domain.Constant.Transaction;

namespace Exon.Recab.Service.Model.PaymentModel
{
    public class GatewayInitializeModel
    {
        public string rfid { get; set; }
        public int bank { get; set; }
        public string returnUrl { get; set; }
        public string urlCall { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
}
