using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.User
{
    public class SingUpByRoleModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(6,ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        [Compare("repassword")]
        public string password { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(6,ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string repassword { get; set; }

        [EmailAddress]
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(3,ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string firstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(3,ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string lastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(10,ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string mobile { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long roleId { get;  set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public UserGender genderType { get;  set; }
    }
}
