using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Exon.Recab.Domain.Constant.User;

namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class UserPackageCredit : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public long CategoryPurchasePackageTypeId { get; set; }

        [ForeignKey("CategoryPurchasePackageTypeId")]
        public virtual CategoryPurchasePackageType CategoryPurchasePackageType { get; set; }

        [Required]
        public DateTime InsertTime { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }

        [Required]
        public long BaseQuota { get; set; }


        [Required]
        public long UsedQuota { get; set; }



        [Required]

        public UserCreditStatus Status { get; set; }

        public virtual List<Product> Prodouct { get; set; }
        

        public UserPackageCredit()
        {
            Prodouct = new List<Product>();
        } 


    }
}
