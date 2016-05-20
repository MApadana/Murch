using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;



namespace Exon.Recab.Api.Models.Article
{
    public class ArticleStructureFindModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long articleStructureId { get;  set; }
    }
}
