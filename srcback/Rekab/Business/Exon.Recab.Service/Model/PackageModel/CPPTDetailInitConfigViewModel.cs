using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PackageModel
{
    public class CPPTDetailInitConfigViewModel
    {

        public string title { get; set; }

        public long purchasePackageTypeId { get; set; }

        public List<PackageInitConfigItemViewModel> configItems { get; set; }

        public CPPTDetailInitConfigViewModel()
        {
            configItems = new List<PackageInitConfigItemViewModel>();
        }

    }
}
