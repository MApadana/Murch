using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Package
{
    public class SearchVoucherModel
    {
        public string responseCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public VoucherStatus status { get; set; }

        public string title { get; set; }

        public string toPersianDate { get; set; }

        public string fromPersianDate { get;  set; }
       
        public int pageIndex { get;  set; }


        public int pageSize { get;  set; }

    }

    public enum VoucherStatus
    {
     استفاده_شده=1,
     استفاده_نشده=2,
     همه=0
    }
}
