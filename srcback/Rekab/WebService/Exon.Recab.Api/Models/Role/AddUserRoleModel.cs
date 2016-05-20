using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.Role
{
    public class AddUserRoleModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long cumUserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public List<long> roleIds { get; set; }

        public AddUserRoleModel()
        {
            roleIds = new List<long>();
        }
    }
}
