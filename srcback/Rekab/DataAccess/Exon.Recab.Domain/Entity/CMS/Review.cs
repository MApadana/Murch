using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity.CMS
{
    public class Review : BaseEntity
    {
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [MaxLength(1000)]
        public string Body { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        public long VisitCount { get; set; }
             
    }
}
