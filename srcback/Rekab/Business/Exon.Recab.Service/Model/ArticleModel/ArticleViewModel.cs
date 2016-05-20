using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ArticleModel
{
    public class ArticleViewModel
    {

        public long articleId { get; set; }

        public string title { get; set; }

        public string createTime { get;  set; }
        
        public string htmlContent { get; set; }

        public float rate { get;  set; }

        public long visitCount { get;  set; }

        public float userRate { get;  set; }

        public string briefDescription { get; set; }

        public string logoUrl { get; set; }

        public List<ArticleStructureFeatureViewModel> searchFilterItems { get; set; }

        public List<ArticleStructureViewModel> parent { get; set; }
        

        public ArticleViewModel()
        {
            searchFilterItems = new List<ArticleStructureFeatureViewModel>();
            parent = new List<ArticleStructureViewModel>();
        }


    }
}
