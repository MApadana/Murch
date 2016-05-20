using Exon.Recab.Api.Infrastructure.Resource;
using Exon.Recab.Api.Utility.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Public
{
    public class SearchConfig
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public SearchType searchType { get; set; }
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool isSimple { get; set; }

    }
}
