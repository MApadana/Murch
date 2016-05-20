using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.Article
{
    public class ArticleSearchModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        public string keyword { get; set; }
       
        public long? articleStructureId { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }
    }
}
