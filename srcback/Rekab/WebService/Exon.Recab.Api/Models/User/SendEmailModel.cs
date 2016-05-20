using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.User
{
    public class SendEmailModel
    {
        [EmailAddress(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Email")]
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string email { get;  set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public VerifyType type { get; set; }
    }
}