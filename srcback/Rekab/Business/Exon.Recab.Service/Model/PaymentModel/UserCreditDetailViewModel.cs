using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PaymentModel
{
   public  class UserCreditDetailViewModel
    {
        public List<CreditDetailItemViewModel> items { get; set; }

        public string totalAmount { get; set; }
        public UserCreditDetailViewModel()
        {
            items = new List<CreditDetailItemViewModel>();
        }
    }
}
