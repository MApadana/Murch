using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Exon.Recab.Api.Infrastructure.Resource;

namespace Exon.Recab.Api.Models.Advertise
{
    public class AdvertiseSearchModel
    {
        public long userId { get; set; }
       
        public string keyword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        //rename : selectedItems 
        public List<SelectItemFilterSearchModel> selectedItems { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public SearhcType searchType { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public VisitType visitType { get; set; }

        public SortType sortType { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }

        public AdvertiseSearchModel()
        {
            selectedItems = new List<SelectItemFilterSearchModel>();
        }
    }

    public enum SearhcType
    {
        advertise = 0,
        alert = 1

    }

    public enum VisitType
    {
        visit = 2,
        notVisit = 1,
        all = 0
    }

    public enum SortType
    {
       جدیدترین=0,
       پربازدیدترین=1
    }

}
