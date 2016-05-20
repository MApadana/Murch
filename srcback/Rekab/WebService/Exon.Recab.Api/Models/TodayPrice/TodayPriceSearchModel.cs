using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.TodayPrice
{
    public class TodayPriceSearchModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]

        //rename : selectedItems
        public List<TodayPriceSearchFilterItem> selectedItems { get; set; }
      
        public long categoryId { get; set; }

        public string keyword { get; set; }

        //rename : pageSize
        public int pageSize { get; set; }

        //rename : pageIndex
        public int pageIndex { get; set; }
    

        public TodayPriceSearchModel()
        {
            selectedItems = new List<TodayPriceSearchFilterItem>();
        }
    }
}
