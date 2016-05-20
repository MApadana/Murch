using System;
using System.Collections.Generic;

namespace Exon.Recab.Service.Model.TodayPriceModel
{
    public class AdminTodayPriceViewModel
    {
        public string sellOption { get;  set; }        
        public long todayPriceId { get;  set; }
        public string title { get;  set; }
        public long price { get; set; }
        public string lastDate { get; set; }
        public List<TodayPriceCategoryFeatureEditViewModel> categoryFeatures { get; set; }
        public long dealershipPrice { get;  set; }

        public AdminTodayPriceViewModel()
        {
            categoryFeatures = new List<TodayPriceCategoryFeatureEditViewModel>();
            
        }
    }
}
