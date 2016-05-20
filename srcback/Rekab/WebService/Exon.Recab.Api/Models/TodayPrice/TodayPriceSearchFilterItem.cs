
using System.Collections.Generic;


namespace Exon.Recab.Api.Models.TodayPrice
{
    public class TodayPriceSearchFilterItem
    {
        public long categoryFeatureId { get;  set; }

        public List<long> featureValueIds { get; set; }

        public TodayPriceSearchFilterItem()
        {
            featureValueIds = new List<long>();
        }
    }
}
