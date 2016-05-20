using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel.AlertModel
{
    public class AlertSingleViewModel
    {
        public long countTotal { get; set; }

        public long visitCount { get; set; }
       
        public string title { get;  set; }

        public string description { get; set; }
        public string logoUrl { get;  set; }
        public long id { get;  set; }
        public long categoryId { get; set; }

        //rename2 : alertCategoryFeatures
        public List<AlertProductCategoryFeatureDetailViewModel> categoryFeature { get; set; }
        public AlertSingleViewModel()
        {
            categoryFeature = new List<AlertProductCategoryFeatureDetailViewModel>();
        }
    }
}
