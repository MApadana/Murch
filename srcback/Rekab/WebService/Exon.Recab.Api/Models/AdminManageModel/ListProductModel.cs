using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.AdminManageModel
{
    public class ListProductModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }


        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public ProductAdminStatus status { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public FeedbackAdminStatus feedBack { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public DealershipAdminStatus dealership { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }
    }

    public enum ProductAdminStatus
    {

        فعال = 0,
        غیرفعال = 1,
        انتظاربرای_تایید = 2,
        آرشیو = 3,
        غیرفعال_بروزرسانی = 4
    }

    public enum FeedbackAdminStatus
    {

        همه = 0,
        دارد = 1,
        ندارد = 2,

    }

    public enum DealershipAdminStatus
    {
        همه = 0,
        دارد = 1,
        ندارد = 2,
    }

}
