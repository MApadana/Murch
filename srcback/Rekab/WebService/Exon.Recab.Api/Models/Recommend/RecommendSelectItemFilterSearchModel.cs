using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Recommend
{
    public class RecommendSelectItemFilterSearchModel
    {

        public long categoryFeatureId { get; set; }

        public List<long> SelectedFeatureValue { get; set; }

        public string customValue { get; set; }

        public RecommendSelectItemFilterSearchModel()
        {
            SelectedFeatureValue = new List<long>();
        }
    }
}
