using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.AdminManageModel
{
    public class ChangeStatusProductModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public ProductAdminStatus status { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long advertiseId { get; set; }
    }
}
