using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Exon.Recab.Api.Infrastructure.Resource;

namespace Exon.Recab.Api.Models.Recommend
{
    public class GetBriefSearchModel
    {
        [MinLength(2)]
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string key { get; set; }
    }
}
