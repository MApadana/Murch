using Exon.Recab.Domain.Constant.Prodoct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Domain.Entity
{
    public class FeedbackProduct : BaseEntity
    {
        [MaxLength(500)]
        public string UserComment { get; set; }

        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string CategoryFeatureTitle { get; set; }

        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public FeedBackStatus Status { get; set; }

    }
}
