using System.Collections.Generic;


namespace Exon.Recab.Api.Models.Article
{
    public class EditArticleModel
    {
        public long userId { get; set; }

        public long articleId { get; set; }

        public long articleStructureId { get; set; }

        public string title { get; set; }

        public string briefDescription { get; set; }

        public string htmlContent { get; set; }

        public bool featureValueUpdateStatus { get; set; }

        public string logoUrl { get; set;  }

        public List<ArticleStructureFeatureSelectItemModel> selectedItems { get; set; }

        public EditArticleModel()
        {
            selectedItems = new List<ArticleStructureFeatureSelectItemModel>();
        }

    }
}
