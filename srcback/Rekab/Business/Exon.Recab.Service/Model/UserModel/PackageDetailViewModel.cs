using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.UserModel
{
    public class PackageDetailViewModel
    {
        public long baseCount { get; set; }

        public long usedCount { get; set; }

        public List<PackageConfigDetailViewModel> configs { get; set; }
        public string logoUrl { get; internal set; }

        public PackageDetailViewModel()
        {
            configs = new List<PackageConfigDetailViewModel>();
        }
    }
}
