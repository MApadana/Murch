using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class Voucher:BaseEntity
    {
        [Index(IsUnique =true)]
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(40)]
        public string ResponseCode { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public long? CreditId { get; set; }

        [ForeignKey("CreditId")]
        public virtual Credit Credit { get; set; }

        [Required]
        public long VoucherConfigId { get; set; }

        [ForeignKey("VoucherConfigId")]
        public virtual VoucherConfig VoucherConfig { get; set; }
    }
}
