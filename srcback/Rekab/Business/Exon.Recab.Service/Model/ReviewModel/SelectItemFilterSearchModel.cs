using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ReviewModel
{
    public class SelectItemFilterSearchModel
    {
        public long categoryFeatureId { get; set; }

        public List<long> selectedFeatureValues { get; set; }

        public string customValue { get; set; }

        public SelectItemFilterSearchModel()
        {
            selectedFeatureValues = new List<long>();
        }
    }
}
