using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.AdminManageModel
{
    public class AdminEditDealershipModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long cumUserId { get; set; }

        public string address { get;  set; }
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public List<long> categoryIds { get;  set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long cityId { get;  set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long dealershipId { get;  set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public DealershipStatus status { get; set; }

        public string description { get;  set; }

        public string fax { get;  set; }

        public double lat { get;  set; }

        public double lng { get;  set; }

        public string logoUrl { get;  set; }

        public string tell { get;  set; }

        public string title { get;  set; }

        public string websiteUrl { get;  set; }

        public AdminEditDealershipModel()
        {
            categoryIds = new List<long>();
        }
    }

    

}
