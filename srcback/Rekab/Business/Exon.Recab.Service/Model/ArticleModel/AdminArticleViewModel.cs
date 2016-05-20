
using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ArticleModel
{
    public class AdminArticleViewModel
    {

        public long articleId { get; set; }

        public long articleStructureId { get; set; }

        public string title { get; set; }

        public string briefDescription { get; set; }

        public string htmlContent { get;  set; }        
        
        public List<ArticleStructureFeatureEditViewModel> categoryFeatures { get; set; }

        public long categoryId { get; set; }

        public string categoryTitle { get; set; }

        public string logoUrl { get; set; }

        public AdminArticleViewModel()
        {
            categoryFeatures = new List<ArticleStructureFeatureEditViewModel>();
        }
    }
}
