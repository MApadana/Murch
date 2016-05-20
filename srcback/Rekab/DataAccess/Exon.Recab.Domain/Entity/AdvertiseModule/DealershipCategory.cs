using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class DealershipCategory: BaseEntity
    {
        [Required]
        public long DealershipId { get; set; }

        [ForeignKey("DealershipId")]
        public virtual Dealership Dealership { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
