using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ReviewModel
{
  public  class ReviewCategoryFeatureViewModel
    {
        public List<ReviewFeatureValueViewModel> featureValues { get; set; }

        public long categoryFeatureId { get; set; }

        public string title { get; set; }

        public ReviewCategoryFeatureViewModel()
        {
            featureValues = new List<ReviewFeatureValueViewModel>(); 
        } 
    }
}
