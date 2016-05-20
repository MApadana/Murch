using Exon.Recab.Domain.Constant.Transaction;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Exon.Recab.Domain.Entity
{
    public class Transaction : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public BankStatus Status { get; set; }

        [MaxLength(50)]
        public string RfId { get; set; }


        [Required]
        public BankType BankType { get; set; }

        [MaxLength(50)]
        public string BankResponse { get; set; }

        [Required]
        public long Amount { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
