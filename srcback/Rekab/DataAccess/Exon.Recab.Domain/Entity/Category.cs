using Exon.Recab.Domain.Entity.PackageModule;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity
{
	public class Category :BaseEntity
	{
        [Required]
        public string Title { get; set; }

        public long? ParentId { get; set; }

        [Required]
        public int TodayPriceChartLastRange { get; set; }

        [Required]
        public int ReletiveCount { get; set; }
        public virtual List<CategoryFeature> CategoryFeatures { get; set; }

        public virtual List<CategoryPurchaseType> CategoryPurchaseTypes { get; set; }

        public virtual List<CategoryExchange> CategoryExchanges { get; set; }

        public Category()
		{
			CategoryFeatures = new List<CategoryFeature>();

            CategoryPurchaseTypes = new List<CategoryPurchaseType>();
		}
	}
}
