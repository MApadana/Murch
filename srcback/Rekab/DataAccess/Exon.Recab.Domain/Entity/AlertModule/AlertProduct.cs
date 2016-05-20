using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Exon.Recab.Domain.Constant.Prodoct;

namespace Exon.Recab.Domain.Entity.AlertModule
{
    public class AlertProduct : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [Required]
        [MaxLength(3000)]
        public string Description { get; set; }

        [Required]
        public AlertProductStatus Status { get; set; }

        [Required]
        public DateTime InsertDate { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }

        [Required]
        public bool SendEmail { get; set; }

        public bool SendSMS { get; set; }

        public bool SendPush { get; set; }


    }
}
