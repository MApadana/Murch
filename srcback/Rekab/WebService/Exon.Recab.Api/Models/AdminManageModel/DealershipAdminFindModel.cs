using Exon.Recab.Api.Infrastructure.Resource;

using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.AdminManageModel
{
    public class DealershipAdminFindModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public DealershipStatus status { get; set; }
       
        public string title { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public long? cityId { get;  set; }

        public long? stateId { get; set; }
    }
    
}
