using System.Collections.Generic;

namespace Exon.Recab.Service.Model.ReviewModel
{
    public class ReviewCategoryFeature
    {
        public long CategoryFeatureId { get; set; }

        public List<long> FeatureValueId { get; set; }

        public string CustomValue { get; set; }

    }
}
