using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class ProcuctSearchDetailModel
    {
        public long Id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string tell { get; set; }

        public string confirmDate { get; set; }

        public string userName { get; set; }

        public string email { get; set; }

        public List<string> mediaUrl { get; set; }

        public string categoryTitle { get; set; }

        public long categoryId { get; set; }

        public bool hasDealership { get; set; }

        public bool hasReview { get; set; }

        public bool visit { get; set; }

        public bool isFavorite { get; set; }

        public ProcuctDealershipDetailModel dealershipInfo { get; set; }

        public List<ProductCategoryFeatureDetailViewModel> advertiseCategoryFeatures { get; set; }

        public List<TodayPriceSearchFilterViewModel> todayPriceSearchFilterItems { get; set; }
        
        public List<TodayPriceSearchFilterViewModel> reviewSearchFilterItems { get; set; }
        public string articleSearchKeyword { get;  set; }

        public ProcuctSearchDetailModel()
        {
            advertiseCategoryFeatures = new List<ProductCategoryFeatureDetailViewModel>();

            mediaUrl = new List<string>();

            dealershipInfo = new ProcuctDealershipDetailModel();

            todayPriceSearchFilterItems = new List<TodayPriceSearchFilterViewModel>();

            reviewSearchFilterItems = new List<TodayPriceSearchFilterViewModel>();
        }


    }
}
