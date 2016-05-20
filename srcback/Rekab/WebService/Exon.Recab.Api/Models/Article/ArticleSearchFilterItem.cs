using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Article
{
    public class ArticleSearchFilterItem
    {
        public long categoryFeatureId { get;  set; }

        public List<long> featureValueIds { get; set; }

        public ArticleSearchFilterItem()
        {
            featureValueIds = new List<long>();
        }
    }
}
