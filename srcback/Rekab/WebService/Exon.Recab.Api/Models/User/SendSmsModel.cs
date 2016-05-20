using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.User
{
    public class SendSmsModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(8,ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string mobile { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public VerifyType type { get; set; }
    }

    public enum VerifyType {

       ایجادکاربر=0,
       فراموشی_رمز=1,
       تایید_تلفن_همراه=2
    }
}
