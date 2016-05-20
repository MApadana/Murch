using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class CategoryPurchasePackageType : BaseEntity
    {
        [Required]
        public long CategoryPurchaseTypeId { get; set; }

        [ForeignKey("CategoryPurchaseTypeId")]
        public virtual CategoryPurchaseType CategoryPurchaseType { get; set; }

        [Required]
        public long PackageTypeId { get; set; }

        [Required]
        public int OrderId { get; set; }

        public virtual PackageType PackageType { get; set; }

        public virtual List<PurchaseConfig> PurchaseConfig { get; set; }

        public CategoryPurchasePackageType()
        {
            PurchaseConfig = new List<PurchaseConfig>();
        }
    }
}
