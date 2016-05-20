using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ReviewModel
{
   public class ReviewGroupByViewModel
    {
        public long categoryFeatureId { get; set; }

        public List<ReviewLogoItemViewModel> featureValues { get; set; }

        public ReviewGroupByViewModel()
        {
            featureValues = new List<ReviewLogoItemViewModel>();
        }
    }
}
