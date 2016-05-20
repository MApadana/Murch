using System.Collections.Generic;


namespace Exon.Recab.Api.Models.TodayPrice
{
    public class TodayPriceCategoryFeatureSelectItemModel
    {
        
        public long categoryFeatureId { get; set; }

        public List<long> featureValueIds { get; set; }

        public string customValue { get; set; }

        public TodayPriceCategoryFeatureSelectItemModel()
        {
            featureValueIds = new List<long>();
        }

    }
}
