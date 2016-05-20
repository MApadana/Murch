using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity.CMS
{
    public class TodayPriceOption : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]

        public virtual Category Category { get; set; }



    }
}
