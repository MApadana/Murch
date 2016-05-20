using Exon.Recab.Domain.Constant.SMS;
using System;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Domain.Entity
{
    public class SMS : BaseEntity
    {
        [MaxLength(12)]
        [MinLength(10)]
        public string MobileNumber { get; set; }

        [MaxLength(250)]
        public string Content { get; set; } 

        public DateTime SendDate { get; set; }

        [Required]
        public SMSType Type { get; set; }
    }
}
