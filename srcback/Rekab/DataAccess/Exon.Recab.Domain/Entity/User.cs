using System.ComponentModel.DataAnnotations;
using Exon.Recab.Domain.Constant.User;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Exon.Recab.Domain.Entity.PackageModule;

namespace Exon.Recab.Domain.Entity
{
    public class User : BaseEntity
    {

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(40)]
        public string Password { get; set; }

        [Required]
        public UserStatus Status { get; set; }

        [Required]
        [MaxLength(60)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(60)]
        public string LastName { get; set; }

        [Required]
        [MinLength(10)]
        public string Mobile { get; set; }

        [Required]
        public UserGender GenderType { get; set; }

        [Required]
        public bool MobileVerified { get; set; }

        [Required]
        public bool EmailVerified { get; set; }

        public DateTime LastLoginRequest { get; set; }

        public DateTime LastSuccessLogin { get; set; }

        public short UnsuccessTryCount { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }


        public virtual List<UserToken> UserTokens { get; set; }


        public virtual List<Dealership> Dealerships { get; set; }


        public virtual List<UserPackageCredit> UserPackageCredits { get; set; }

        public virtual List<Credit> Credit { get; set; }

        public virtual List<Transaction> Transaction { get; set; }
        public User()
        {
            UserRoles = new List<UserRole>();
            UserTokens = new List<UserToken>();
            Dealerships = new List<Dealership>();
            UserPackageCredits = new List<UserPackageCredit>();
            Transaction = new List<Transaction>();
            Credit = new List<Entity.Credit>();
        }

    }
}
