
using System.Collections.Generic;

namespace Exon.Recab.Service.Model.ArticleModel
{
    public class ArticleStructureFeatureEditViewModel
    {
        public long categoryFeatureId { get; set; }

        public string title { get; set; }

        public List<ArticleFeatureValueEditViewModel> featureValue { get; set; }

        public ArticleStructureFeatureEditViewModel()
        {
            this.featureValue = new List<ArticleFeatureValueEditViewModel>();
        }
    }
}
