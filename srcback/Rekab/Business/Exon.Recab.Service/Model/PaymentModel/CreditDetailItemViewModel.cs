using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PaymentModel
{
    public class CreditDetailItemViewModel
    {
        public string voucher { get; set; }

        public string bank { get; set; }

        public string amount { get; set; }

        public string date { get; set; }

        public string description { get; set; }

        public long id { get; set; }

    }
}
