using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Package
{
    public class EditCategoryPurchasePackageTypeConfigItemModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string configTitle { get;  set; }


        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string value { get;set; }
    }
}
