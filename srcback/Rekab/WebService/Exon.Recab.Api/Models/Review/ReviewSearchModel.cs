using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.Review
{
    public class ReviewSearchModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        public string keyword { get; set; }

        //pagesize
        public int pageSize { get; set; }
         //pageindex
        public int pageIndex { get; set; }
      
        //rename : selectedItems
        public List<ReviewSearchFilterItem> selectedItems { get; set; }

        public ReviewSearchModel()
        {
            selectedItems = new List<ReviewSearchFilterItem>();
        }
    }
}
