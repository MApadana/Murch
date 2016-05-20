using Exon.Recab.Api.Models.Advertise;
using System.Collections.Generic;


namespace Exon.Recab.Api.Models.Review
{
   public class AddReviewModel
    {
        public long userId { get; set; }      

        public string body { get; set; } 

        public long categoryId { get; set; }

        public List<ReviewCategoryFeatureSelectItemModel> selectItem { get; set; }

        public List<AddAdvertiseMediaModel> media { get; set; }

        public AddReviewModel()
        {
            selectItem = new List<ReviewCategoryFeatureSelectItemModel>();
        }
    }
}
