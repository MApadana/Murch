using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Domain.Entity.CMS
{
    public class TodayPriceConfig : BaseEntity
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

        [Required]
        [MaxLength(500)]
        public string SellOption { get; set; }

        [Required]
        public long Price { get; set; }       

        public long DealershipPrice { get; set; }

        [Required]
        public double Tolerance { get; set; }

        [Required]
        public DateTime LastUpdateDate { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public long VisitCount { get; set; }
    }
}
