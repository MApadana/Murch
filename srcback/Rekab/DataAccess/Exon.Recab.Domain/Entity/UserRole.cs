using System.ComponentModel.DataAnnotations;
using Exon.Recab.Domain.Constant.User;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class UserRole : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public long RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }   

        public RoleStatus Status { get; set; }
    }
}
