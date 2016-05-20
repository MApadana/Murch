using Exon.Recab.Api.Infrastructure.Resource;

using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Package
{
    public class VoucherConfigSearchModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]

        public VoucherConfigType status { get; set; }

        public string fromDate { get; set; }

        public string toDate { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }
    }

    public enum VoucherConfigType
    {
        باموفقییت = 0,
        باخطا = 1,
        درحال_تولید = 2,
        خطادرتولیدفایل = 3
    }
}

