using System.Collections.Generic;


namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class PackageType: BaseEntity
    {
        public string Title { get; set; }
        
        public virtual List<CategoryPurchasePackageType> CategoryPurchasePackageTypes { get; set; }

        public PackageType()
        {
            this.CategoryPurchasePackageTypes = new List<CategoryPurchasePackageType>();
        }
    }
}
