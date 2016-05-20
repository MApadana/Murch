using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class SearchWithAggregateResultModel
    {
        public string androidType { get; set; }

        public List<long> selectedFeatureValue { get; set; }

        public string customValue { get; set; }

        public List<FeatuerValueAggregateViewModel> aggregate { get; set; }
        public string title { get; set; }

        public long categoryFeatureId { get; set; }

        public SearchWithAggregateResultModel()
        {
            selectedFeatureValue = new List<long>();
            aggregate = new List<FeatuerValueAggregateViewModel>();
        }

    }
}
