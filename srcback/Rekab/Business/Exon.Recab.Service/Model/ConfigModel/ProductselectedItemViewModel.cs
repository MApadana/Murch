
using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ConfigModel
{
   public class ProductselectedItemViewModel
    {
        public long categoryFeatureId { get; set; }

        public List<long> featureValueIds { get; set; }

        public string customValue { get; set; }

        public ProductselectedItemViewModel()
        {
            featureValueIds = new List<long>();
            customValue = "";
        }
    }
}
