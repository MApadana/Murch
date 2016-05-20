using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class PackageBaseConfigValue : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Value { get; set; }

        [Required]
        public long PackageBaseConfigId { get; set; }

        [ForeignKey("PackageBaseConfigId")]
        public virtual PackageBaseConfig PackageBaseConfig { get; set; }
    }
}
