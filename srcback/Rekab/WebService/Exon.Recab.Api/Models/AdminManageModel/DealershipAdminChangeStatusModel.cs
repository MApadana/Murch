using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.AdminManageModel
{
    public class DealershipAdminChangeStatusModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long dealershipId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public DealershipStatus status { get; set; }
    }

    public enum DealershipStatus
    {
        فعال = 1,
        غیر_فعال = 2,
        انتظار_بروزرسانی=3,
        مسدود=4

    }
}
