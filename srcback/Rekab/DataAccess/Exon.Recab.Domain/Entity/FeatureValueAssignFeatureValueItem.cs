using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class FeatureValueAssignFeatureValueItem : BaseEntity
    {
        [Required]
        public long FeatureValueAssignId { get; set; }

        [ForeignKey("FeatureValueAssignId")]
        public virtual FeatureValueAssign FeatureValueAssign { get; set; }

        [Required]
        public long FeatureValueId { get; set; }

        [ForeignKey("FeatureValueId")]
        public virtual  FeatureValue FeatureValue { get; set; }


    }
}
