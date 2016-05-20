using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class CFAggregateViewModel
    {
        public long categoryFeatureId { get; set; }

        public string title { get; set; }
       
        //rename featureValues
        public List<FeatuerValueAggregateViewModel> featureValues { get; set; }     

        public long totalCount { get; set; }

        public CFAggregateViewModel()
        {
            featureValues = new List<FeatuerValueAggregateViewModel>();
        }
    }
}
