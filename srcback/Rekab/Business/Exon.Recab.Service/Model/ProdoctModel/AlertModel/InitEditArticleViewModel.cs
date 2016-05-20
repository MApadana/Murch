using Exon.Recab.Service.Model.ConfigModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel.AlertModel
{
    public class InitEditAlertViewModel
    {

        
        public List<CategoryFeatureValuesResultModel> categoryFeatureValues { get; set; }
        public long categoryId { get; set; }
        public string expireDate { get; set; }
        public string insertDate { get; set; }
        //rename2:selectedItems
        public List<ProductselectedItemViewModel> selectedItem { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public bool sendSMS { get; set; }
        public bool sendEmail { get; set; }
        public bool sendPush { get; internal set; }

        public InitEditAlertViewModel()
        {
            categoryFeatureValues = new List<CategoryFeatureValuesResultModel>();
            selectedItem = new List<ProductselectedItemViewModel>();
        }
    }
}
