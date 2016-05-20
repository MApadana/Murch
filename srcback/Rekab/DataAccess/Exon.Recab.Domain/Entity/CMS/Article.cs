using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity.CMS
{
    public class Article : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public long ArticleStructureId { get; set; }

        [ForeignKey("ArticleStructureId")]
        public virtual ArticleStructure ArticleStructure { get; set; }

   
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string LogoUrl { get; set; }

        [MaxLength(500)]
        public string BrifDescription { get; set; }

        [Required]
        
        public string Body { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        public long VisitCount { get; set; }

       
    }
}
