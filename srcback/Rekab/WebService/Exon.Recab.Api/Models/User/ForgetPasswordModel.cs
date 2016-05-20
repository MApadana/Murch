using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.User
{
    public class ForgetPasswordModel
    {

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(length: 6, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        [Compare(otherProperty: "rePassword", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "ComparePassword")]
        public string newPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(6, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string rePassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long forgetUserId { get; set; }
    }
}