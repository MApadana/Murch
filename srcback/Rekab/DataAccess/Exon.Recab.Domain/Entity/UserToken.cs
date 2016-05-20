using Exon.Recab.Domain.Constant.Public;
using Exon.Recab.Domain.Constant.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Exon.Recab.Domain.Entity
{
    public class UserToken : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [MaxLength(25)]
        public string Token { get; set; }

        [Required]
        public TokenType TokenType { get; set; }

        [Required]
        public DateTime InsertTime { get; set; }

        [Required]
        public DateTime LastUsedTime { get; set; }

        [Required]
        public ClientType ClientType { get; set; }

        [Required]
        public bool Available { get; set; }
       
    }
}
