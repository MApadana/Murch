using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.EmailModel.SendEmailModel
{
    public class SendPurchaseDetailEmailModel
    {

        public List<PurchaseConfigItemEmailModel> Configs { get; set; }

        public string Name { get;  set; }

        public string CPPTTitle { get;  set; }

        public SendPurchaseDetailEmailModel()
        {
            Configs = new List<PurchaseConfigItemEmailModel>();
        }

    }
}
