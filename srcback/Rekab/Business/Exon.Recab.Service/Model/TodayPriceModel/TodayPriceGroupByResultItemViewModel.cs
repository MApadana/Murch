using System.Collections.Generic;


namespace Exon.Recab.Service.Model.TodayPriceModel
{
    public class TodayPriceGroupByResultItemViewModel
    {
        public long featureValueId { get; set; }

        public string featureValueTitle { get; set; }

        public string logoUrl { get; set; }
      
        public string categoryTitle { get;  set; }

        public long categoryId { get;  set; }

        //rename : priceItems
        public List<TodayPriceSearchResultItemViewModel> priceItems { get; set; }

        public TodayPriceGroupByResultItemViewModel()
        {
            priceItems = new List<TodayPriceSearchResultItemViewModel>();
        }
    }
}
