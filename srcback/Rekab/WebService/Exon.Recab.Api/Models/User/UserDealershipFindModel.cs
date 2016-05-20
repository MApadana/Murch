using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.User
{
    public class UserDealershipFindModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        public long? categoryId { get; set; }

        public UserDealershipStatus status { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }

    }

    public enum UserDealershipStatus
    {
        فعال = 1,
        غیر_فعال = 2,
        انتظار_بروزرسانی = 3

    }
}
