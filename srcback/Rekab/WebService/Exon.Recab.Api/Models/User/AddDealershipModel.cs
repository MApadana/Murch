using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.User
{
    public class AddDealershipModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MaxLength(length: 100, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MaxLength")]
        public string title { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MaxLength(length: 400, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MaxLength")]
        public string address { get;  set; }

        [MaxLength(length: 300, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MaxLength")]
        public string description { get;  set; }

        [MaxLength(length: 20, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MaxLength")]
        public string fax { get;  set; }


        public double lat { get;  set; }

        public double lng { get;  set; }


        [MaxLength(length: 500, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MaxLength")]
        public string logoUrl { get;  set; }


        [MaxLength(length: 200, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MaxLength")]
        public string websiteUrl { get;  set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MaxLength(length: 50, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MaxLength")]
        public string tell { get;  set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public List<long> categoryIds { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long cityId { get;  set; }

        public AddDealershipModel()
        {
            lng = 0;
            lat = 0;
            categoryIds = new List<long>();
        }
    }
}
