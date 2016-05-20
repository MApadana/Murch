using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Exon.Recab.Domain.Entity.CMS
{
    public class ArticleStructure : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        
        public long? ParentArticleStructureId { get; set; }

        [ForeignKey("ParentArticleStructureId")]
        public virtual ArticleStructure ParentArticleStructure { get; set; }

        [MaxLength(200)]
        public string LogoUrl { get; set; }

        public virtual List<Article> Articles { get; set; }

        public ArticleStructure()
        {
            Articles = new List<Article>();
        }

    }
}
