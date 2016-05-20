using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PackageModel
{
    public class CategoryPurchaseTypeViewModel
    {      
        public long categoryPurchaseTypeId { get;  set; }
        public string title { get; set; }

        public bool isFree { get; set; }

        public bool isDealership { get; set; }
    }
}
