using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ConfigModel
{
    public class ExchangeProductselectedItemViewModel
    {
        public long categoryFeatureId { get; set; }

        //rename2 : featureValueIds
        public List<long> featureValueIds { get; set; }

        public string customValue { get; set; }


        public ExchangeProductselectedItemViewModel()
        {
            featureValueIds = new List<long>();
        }

    }
}
