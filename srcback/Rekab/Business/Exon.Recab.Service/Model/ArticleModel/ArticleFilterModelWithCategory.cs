using Exon.Recab.Domain.Entity;


namespace Exon.Recab.Service.Model.ArticleModel
{
    public class ArticleFilterModelWithCategory
    {
        public ArticleFilterModel Filter { get; set; }

        public CategoryFeature CategoryFeature  { get; set; }
    }
}
