using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Package
{
    public class PurchasPackageTypeDetail
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryPurchaseId { get; set; }
     
    }
}
