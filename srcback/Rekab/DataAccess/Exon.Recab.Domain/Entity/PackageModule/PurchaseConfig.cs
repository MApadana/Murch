using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class PurchaseConfig : BaseEntity
    {
        [Required]
        public long PackageBaseConfigId { get; set; }

        [ForeignKey("PackageBaseConfigId")]
        public virtual PackageBaseConfig PackageBaseConfig { get; set; }

        [Required]
        public long CategoryPurchasePackageTypeId { get; set; }

        [ForeignKey("CategoryPurchasePackageTypeId")]
        public virtual CategoryPurchasePackageType CategoryPurchasePackageType  { get; set; }

        [Required]
        public string Value { get; set; }

    }
}
