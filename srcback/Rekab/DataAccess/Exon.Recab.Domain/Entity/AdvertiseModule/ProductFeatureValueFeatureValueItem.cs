using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class ProductFeatureValueFeatureValueItem : BaseEntity
    {
        [Required]
        public long ProductFeatureValueId { get; set; }

        [ForeignKey("ProductFeatureValueId")]
        public virtual ProductFeatureValue ProductFeatureValue { get; set; }

        [Required]
        public long FeatureValueId { get; set; }

        [ForeignKey("FeatureValueId")]
        public virtual  FeatureValue FeatureValue { get; set; }


    }
}
