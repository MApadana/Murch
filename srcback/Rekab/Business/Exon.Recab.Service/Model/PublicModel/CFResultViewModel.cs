using System.Collections.Generic;

namespace Exon.Recab.Service.Model.PublicModel
{
    public class CFResultViewModel
    {
        public long categoryFeatureId { get; set; }

        public string title { get; set; }

        public string customValue { get; set; }

        public List<FVResultViewModel> featureValues { get; set; }

        public CFResultViewModel()
        {
            featureValues = new List<FVResultViewModel>();
        }
    }
}
