using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Package
{
    public class EditCategoryPurchasePackageTypeConfigModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryPurchasePackageTypeId { get; set; }


        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public List<EditCategoryPurchasePackageTypeConfigItemModel> configItems { get; set; }

        public EditCategoryPurchasePackageTypeConfigModel()
        {
            configItems = new List<EditCategoryPurchasePackageTypeConfigItemModel>();
        }
    }
}
