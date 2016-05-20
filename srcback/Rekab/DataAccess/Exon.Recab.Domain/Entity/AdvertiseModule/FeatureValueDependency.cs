

using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class FeatureValueDependency : BaseEntity
    {
        public long FeatureValueId { get; set; }

        [ForeignKey("FeatureValueId")]
        public virtual FeatureValue FeatureValue { get; set; }


        public long? FeatureValueParentId { get; set; }

        [ForeignKey("FeatureValueParentId")]
        public virtual FeatureValue FeatureValueParent { get; set; }

        public long? FeatureValueChildId { get; set; }

        [ForeignKey("FeatureValueChildId")]
        public virtual FeatureValue FeatureValueChild { get; set; }

        public long? CategoryFeatureHideId { get; set; }

        [ForeignKey("CategoryFeatureHideId")]
        public virtual CategoryFeature CategoryFeatureHide { get; set; }

        public long? CategoryFeatureShowId { get; set; }

        [ForeignKey("CategoryFeatureShowId")]
        public virtual CategoryFeature CategoryFeatureShow { get; set; }

    }
}
