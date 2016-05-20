using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Review
{
    public class CompareReviewModel
    {
        public long userId { get; set; }

        public List<long> reviewIds { get; set; }

        public long categoryId { get; set; }
    }
}
