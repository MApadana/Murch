using System;
using System.Collections.Generic;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Service.Model.AdminModel;
using Exon.Recab.Service.Model.UserModel;

namespace Exon.Recab.Service.Model.ConfigModel
{
    public class InitEditAdvertiseViewModel
    { 
        public string title { get; set; }

        public DateTime insertDate { get; set; }

        public string tell { get; set; }

        public long selectedPackageTypeId { get; set; }

        public long? selectedDealershipId { get; set; }

        public long? exchangeCategoryId { get; set; }

        public string description { get;  set; }

        public PackageDetailViewModel packageDetail { get; set; }

        public List<DealershipViewModel> dealerships { get; set; }

        public List<ProductselectedItemViewModel> selectedItems { get; set; }
        
        public List<CategoryFeatureValuesResultModel> categoryFeatureValues { get; set; }

        public List<ProductMediaEditViewModel> media { get; set; }
        public long categoryId { get;  set; }
        public string maxMediaCount { get; set; }

        public InitEditAdvertiseViewModel()
        {
            categoryFeatureValues = new List<CategoryFeatureValuesResultModel>();
            packageDetail = new PackageDetailViewModel();
            selectedItems = new List<ProductselectedItemViewModel>();
            dealerships = new List<DealershipViewModel>();
            media = new List<ProductMediaEditViewModel>();

        }

    }
}
