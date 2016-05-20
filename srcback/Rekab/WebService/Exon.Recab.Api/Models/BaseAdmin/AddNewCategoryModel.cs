using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.BaseAdmin
{
    public class AddNewCategoryModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string title { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public int todayPriceChartlastRange { get; set; }

        public long? parentId { get; set; }
        public int relativeCount { get;  set; }
    }
}