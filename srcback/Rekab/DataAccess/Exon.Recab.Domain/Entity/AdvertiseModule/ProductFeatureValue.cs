using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Exon.Recab.Domain.Entity
{
    public class ProductFeatureValue : BaseEntity
    {
        [Required]
        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
       
        public long CategoryFeatureId { get; set; }

        [ForeignKey("CategoryFeatureId")]

        public virtual CategoryFeature CategoryFeature { get; set; }

        [MaxLength(300)]
        public string CustomValue { get; set; }

        public virtual List<ProductFeatureValueFeatureValueItem> ListFeatureValue { get; set; }
        public ProductFeatureValue()
        {            
            ListFeatureValue = new List<ProductFeatureValueFeatureValueItem>();
        }
    }
}
