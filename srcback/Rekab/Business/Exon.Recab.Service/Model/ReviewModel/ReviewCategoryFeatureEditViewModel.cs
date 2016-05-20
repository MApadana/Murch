using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ReviewModel
{
    public class ReviewCategoryFeatureEditViewModel
    {
        public long categoryFeatureId { get; set; }

        public string title { get; set; }

        public List<ReviewFeatureValueEditViewModel> featureValue { get; set; }

        public ReviewCategoryFeatureEditViewModel()
        {
            this.featureValue = new List<ReviewFeatureValueEditViewModel>();
        }
    }
}
