using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.Advertise
{
    public class AdvertiseLocationSearchModel
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

        public double lat { get; set; }

        public double lng { get; set; }

        public int distance { get; set; }

        public int pageSize { get; set; }


        public int pageIndex { get; set; }

        public AdvertiseLocationSearchModel()
        {
            selectedItems = new List<SelectItemFilterSearchModel>();
        }



    }
}