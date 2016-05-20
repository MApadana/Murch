using System.Collections.Generic;


namespace Exon.Recab.Api.Models.Article
{
    public class ArticleStructureFeatureSelectItemModel
    {
        public long categoryFeatureId { get; set; }

        public List<long> featureValueIds { get; set; }

        public string customValue { get; set; }

        public ArticleStructureFeatureSelectItemModel()
        {
            featureValueIds = new List<long>();
        }

    }
}
