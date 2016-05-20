using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Review
{
    public class ReviewCategoryFeatureSelectItemModel
    {
        public long categoryFeatureId { get; set; }

        public List<long> featureValueId { get; set; }

        public string customValue { get; set; }

        public ReviewCategoryFeatureSelectItemModel()
        {
            featureValueId = new List<long>();
        }

    }
}
