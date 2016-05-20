
using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ArticleModel
{
  public  class ArticleStructureFeatureViewModel
    {
        public List<long> featureValueIds { get; set; }

        public long categoryFeatureId { get; set; }


        public ArticleStructureFeatureViewModel()
        {
            featureValueIds = new List<long>(); 
        } 
    }
}
