using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Review
{
   public class FindSingleReviewModel
    {

        public long userId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long reviewId { get; set; }
    }
}
