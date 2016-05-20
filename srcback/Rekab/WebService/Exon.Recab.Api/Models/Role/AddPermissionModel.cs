using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Role
{
    public class AddPermissionModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long roleId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long resourceId { get; set; }
    }
}