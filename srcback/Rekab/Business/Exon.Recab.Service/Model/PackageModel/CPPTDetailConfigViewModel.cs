using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PackageModel
{
    public class CPPTDetailConfigViewModel
    {
        public string logoUrl { get; set; }

        public string title { get; set; }

        public long purchasePackageTypeId { get; set; }

        public List<PackageConfigItemViewModel> packageConfigItems { get; set; }

        public CPPTDetailConfigViewModel()
        {
            packageConfigItems = new List<PackageConfigItemViewModel>();
        }

    }
}
