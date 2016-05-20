
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class ExchangeFeatureValue :BaseEntity
    {
        [Required]
        public long CategoryFeatureId { get; set; }

        [ForeignKey("CategoryFeatureId")]
        public virtual CategoryFeature CategoryFeature { get; set; }

        public long? FeatureValueId { get; set; }

        [ForeignKey("FeatureValueId")]
        public virtual FeatureValue FeatureValue { get; set; }

        [MaxLength(100)]
        public string CustomValue { get; set; }

    }
}
