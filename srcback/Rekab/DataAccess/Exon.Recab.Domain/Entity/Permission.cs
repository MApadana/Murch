using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity
{
    public class Permission : BaseEntity
    {
        [Required]
        public long RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [Required]
        public long ResourceId { get; set; }

        [ForeignKey("ResourceId")]
        public virtual Resource Resource { get; set; }

    }
}
