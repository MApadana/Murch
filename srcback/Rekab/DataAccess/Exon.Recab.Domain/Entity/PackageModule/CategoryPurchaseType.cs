using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class CategoryPurchaseType : BaseEntity
    {
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public long PurchaseTypeId { get; set; }

        [ForeignKey("PurchaseTypeId")]
        public virtual  PurchaseType PurchaseType { get; set; }

        public virtual List<CategoryPurchasePackageType> CategoryPurchasePackageTypes { get; set; }

        public CategoryPurchaseType()
        {
            this.CategoryPurchasePackageTypes = new List<CategoryPurchasePackageType>();
        }
    }
}
