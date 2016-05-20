using Exon.Recab.Domain.Entity.PackageModule;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class Credit : BaseEntity
    {
        public long UserId { get; set; }

        [ForeignKey("UserId")]

        public virtual User User { get; set; }

        public long? TransactionId { get; set; }


        [ForeignKey("TransactionId")]
        public virtual Transaction Transaction { get; set; }


        public long? ParentCreditId { get; set; }

        [ForeignKey("ParentCreditId")]
        public virtual Credit ParentCredit { get; set; }

        public long? VoucherId { get; set; }


        public long? UserPackageCreditId { get; set; }


        [ForeignKey("UserPackageCreditId")]
        public virtual UserPackageCredit UserPackageCredit { get; set; }


        [Required]
        public DateTime InsertTime { get; set; }


        [Required]
        public long Amount { get; set; }


        [MaxLength(200)]
        public string Description { get; set; }


    }
}
