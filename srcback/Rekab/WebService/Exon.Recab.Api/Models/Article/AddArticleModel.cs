using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.Article
{
   public class AddArticleModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string title { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string htmlContent { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string briefDescription { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long articleStructureId { get; set; }

        public string logoUrl { get; set; }
             

        public List<ArticleStructureFeatureSelectItemModel> selectedItems { get; set; }
      

        public AddArticleModel()
        {
            selectedItems = new List<ArticleStructureFeatureSelectItemModel>();
        }
    }
}
