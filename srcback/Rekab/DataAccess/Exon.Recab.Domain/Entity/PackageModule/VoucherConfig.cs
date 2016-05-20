using Exon.Recab.Domain.Constant.Voucher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class VoucherConfig : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public long Value { get; set; }

        [Required]
        public VoucherStatus Status { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public DateTime CreatDate { get; set; }

        [MaxLength(200)]
        public string SourceFileName { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public long Count { get; set; }

        [Required]
        public VoucherCreationStatus CreateStatus { get; set; }

        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual List<Voucher> Vouchers { get; set; }

        public VoucherConfig()
        {
            Vouchers = new List<Voucher>();
        }

     
    }
}
