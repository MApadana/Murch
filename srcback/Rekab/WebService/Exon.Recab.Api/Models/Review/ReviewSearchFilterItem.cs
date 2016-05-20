using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Review
{
    public class ReviewSearchFilterItem
    {
        public long categoryFeatureId { get;  set; }

        public List<long> featureValueIds { get; set; }

        public ReviewSearchFilterItem()
        {
            featureValueIds = new List<long>();
        }
    }
}
