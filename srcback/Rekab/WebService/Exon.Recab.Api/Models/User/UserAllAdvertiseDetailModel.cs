using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.User
{
    public class UserAllAdvertiseDetailModel
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public UserAdvertiseStatus status { get; set; }
    }

    public enum UserAdvertiseStatus
    {
        فعال = 0,
        غیرفعال = 1,
        انتظاربرای_تایید = 2,
        آرشیو = 3,
        غیرفعال_بروزرسانی = 4

    }
}
