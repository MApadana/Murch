using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class TodayPriceSearchFilterViewModel
    {
        public long categoryFeatureId { get; set; }

        public List<long> featureValueIds { get; set; }

        public TodayPriceSearchFilterViewModel()
        {
            featureValueIds = new List<long>();
        }
    }
}
