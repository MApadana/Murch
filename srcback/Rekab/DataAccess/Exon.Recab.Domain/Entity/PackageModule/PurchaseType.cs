using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class PurchaseType :BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [Required]
        public bool AvailableDealership { get; set; }

        [Required]
        public bool IsFree { get; set; }

        [Required]
        public string LogoUrl { get; set; }

        public virtual List<CategoryPurchaseType> CategoryPurchaseTypes { get; set; }

        public PurchaseType()
        {
            CategoryPurchaseTypes = new List<CategoryPurchaseType>();
        }
    }
}
