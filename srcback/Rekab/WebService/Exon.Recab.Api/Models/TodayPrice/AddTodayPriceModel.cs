using Exon.Recab.Api.Infrastructure.Resource;
using Exon.Recab.Api.Models.Advertise;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.TodayPrice
{
   public class AddTodayPriceModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string sellOption { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string persianLastUpdateDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long price { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long dealershipPrice { get; set; }

        public List<TodayPriceCategoryFeatureSelectItemModel> selectedItems { get; set; }
        

        public AddTodayPriceModel()
        {
            selectedItems = new List<TodayPriceCategoryFeatureSelectItemModel>();
        }
    }
}
