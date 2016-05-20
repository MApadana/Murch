using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.User
{
  public  class AddCreditModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        public long? amount { get; set; }

        public string voucherCode { get;  set; }
    }
}
