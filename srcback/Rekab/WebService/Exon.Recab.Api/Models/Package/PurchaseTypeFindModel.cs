using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Package
{
    public class PurchaseTypeFindModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long purchaseTypeId { get; set; }

        public int pageSize { get; set; }
        public int pageIndex { get;  set; }

        
    }
}
