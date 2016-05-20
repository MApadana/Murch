using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.PublicModel
{
   public class SelectItemModel
    {
        public long CategoryFeatureId { get; set; }

        public List<long> FeatureValueIds { get; set; }

        public string CustomValue { get; set; }
    }
}
