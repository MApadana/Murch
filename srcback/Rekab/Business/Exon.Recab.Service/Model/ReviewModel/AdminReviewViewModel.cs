using System;
using System.Collections.Generic;

namespace Exon.Recab.Service.Model.ReviewModel
{
    public class AdminReviewViewModel
    {
        public string body { get;  set; }        
        public long reviewId { get;  set; }
        public string title { get;  set; }

        public List<ReviewCategoryFeatureEditViewModel> categoryFeatures { get; set; }
        public List<string> media { get;  set; }

        public AdminReviewViewModel()
        {
            categoryFeatures = new List<ReviewCategoryFeatureEditViewModel>();
            media = new List<string>();
        }
    }
}
