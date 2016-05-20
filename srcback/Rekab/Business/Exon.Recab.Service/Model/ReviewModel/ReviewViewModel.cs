using Exon.Recab.Service.Model.ProdoctModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ReviewModel
{
    public class ReviewViewModel
    {
        public List<string> mediaUrl;

        public string articleSearchKeyword { get; set; }

        public string categoryTitle { get; set; }

        public long categoryId { get; set; }

        public string htmlContent { get; set; }

        public string createTime { get; set; }

        public long reviewId { get; set; }

        public double rate { get; set; }

        public string title { get; set; }

        public string logoUrl { get;  set; }

        public long visitCount { get; set; }

        public List<ReviewCategoryFeatureViewModel> reviewCategoryFeatures { get; set; }
        
        public List<SelectItemFilterSearchModel> advertiseSearchFilterItems { get; set; }

        public float userRate { get; set; }

         public List<TodayPriceSearchFilterViewModel> todayPriceSearchFilterItems { get; set; }

        public ReviewViewModel()
        {
            reviewCategoryFeatures = new List<ReviewCategoryFeatureViewModel>();
            advertiseSearchFilterItems = new List<SelectItemFilterSearchModel>();
            mediaUrl = new List<string>();
        }

    }
}
