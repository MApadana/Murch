using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Article
{
    public class ArticleStructureSearchModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        public long? parentArticleStructure { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }
    }
}
