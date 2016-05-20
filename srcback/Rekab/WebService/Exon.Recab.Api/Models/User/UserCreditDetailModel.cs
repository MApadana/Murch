using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.User
{
    public class UserCreditDetailModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        public string toPersianDate { get; set; }

        public string fromPersianDate { get; set; }

        public int pageIndex { get;  set; }

        public int pageSize { get; set; }
       
    }
}
