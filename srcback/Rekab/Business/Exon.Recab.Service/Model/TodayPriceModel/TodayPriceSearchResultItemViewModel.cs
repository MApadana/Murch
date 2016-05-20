
using System.Collections.Generic;

namespace Exon.Recab.Service.Model.TodayPriceModel
{
    public class TodayPriceSearchResultItemViewModel
    {
        public string price { get; set; }

        public string tolerance { get; set; }

        public string sellOption { get; set; }

        public long todayPriceId { get; set; }

        public string title { get; set; }

        //must be elimite extra prop
        public List<TodayPriceCategoryFeatureEditViewModel> relatedFeatureValues { get; set; }

        public string dealershipPrice { get;  set; }
        public string imageUrl { get; set; }

        public TodayPriceSearchResultItemViewModel()
        {
            relatedFeatureValues = new List<TodayPriceCategoryFeatureEditViewModel>();
        }
    }
}
