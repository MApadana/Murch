using System;
using System.Collections.Generic;


namespace Exon.Recab.Api.Models.Advertise
{
    public class SelectItemFilterSearchModel
    {
        public long categoryFeatureId { get; set; }

        public List<long> selectedFeatureValues { get; set; }
        
        public string customValue { get; set; }

        public SelectItemFilterSearchModel()
        {
            selectedFeatureValues = new List<long>();
        }
    }
}
