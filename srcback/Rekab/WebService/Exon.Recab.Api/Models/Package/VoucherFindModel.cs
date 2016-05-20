using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Package
{
    public class VoucherFindModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]

        public long voucherConfigId { get; set; }

        public string responceCode { get; set; }

        public int pageIndex { get;  set; }
       
        public int pageSize { get;  set; }
        
    }
}
