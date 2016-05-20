using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.User
{
    public class EditUserModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(3, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string firstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(3, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string lastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public UserGender genderType { get; set; }


    }
}