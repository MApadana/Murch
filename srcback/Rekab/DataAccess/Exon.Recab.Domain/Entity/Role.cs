using System.ComponentModel.DataAnnotations;
using Exon.Recab.Domain.Constant.User;
using System;
using System.Collections.Generic;

namespace Exon.Recab.Domain.Entity
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        public RoleStatus Status { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }


        public virtual List<Permission> Permission { get; set; }

        public Role()
        {
            UserRoles = new List<UserRole>();
            Permission = new List<Permission>();
        }
    }
}
