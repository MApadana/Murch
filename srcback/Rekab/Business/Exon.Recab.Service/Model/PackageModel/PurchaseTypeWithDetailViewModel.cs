using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PackageModel
{
   public class PurchaseTypeWithDetailViewModel
    {
         public string title { get; set; }

        public bool isDealership { get; set; }

        public bool isFree { get; set; }

        public string logoUrl { get; set; }

        public List<CPPTDetailConfigViewModel> items { get; set; }

        public PurchaseTypeWithDetailViewModel()
        {
            items = new List<CPPTDetailConfigViewModel>();
        }
    }
}
