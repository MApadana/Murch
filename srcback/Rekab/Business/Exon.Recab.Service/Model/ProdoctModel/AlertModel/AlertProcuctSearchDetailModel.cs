using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel.AlertModel
{
    public class AlertProcuctSearchDetailModel
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

        public long? reviewId { get; set; }

        public List<ProductCategoryFeatureDetailViewModel> categoryFeature { get; set; }

        public List<ProductCategoryFeatureDetailViewModel> categoryFeatureTodayPrice { get; set; }

        public ProcuctDealershipDetailModel dealershipInfo { get; set; }

        public AlertProcuctSearchDetailModel()
        {
            categoryFeature = new List<ProductCategoryFeatureDetailViewModel>();

            mediaUrl = new List<string>();

            dealershipInfo = new ProcuctDealershipDetailModel();

            categoryFeatureTodayPrice = new List<ProductCategoryFeatureDetailViewModel>();

        }
    }
}
