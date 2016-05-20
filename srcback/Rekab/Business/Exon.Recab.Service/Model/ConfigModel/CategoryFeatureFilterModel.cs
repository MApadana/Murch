using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ConfigModel
{
   public class CategoryFeatureFilterModel
    {
        public long CategoryFeatureId { get; set; }
        public List<long>  FeatureValueId { get; set; }

        public CategoryFeatureFilterModel()
        {
            FeatureValueId = new List<long>();
        }
    }
}
