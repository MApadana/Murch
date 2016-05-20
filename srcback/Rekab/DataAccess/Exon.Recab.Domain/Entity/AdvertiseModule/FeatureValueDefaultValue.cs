using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class FeatureValueDefaultValue : BaseEntity
    {
        public long FeatureValueId { get; set; }

        [ForeignKey("FeatureValueId ")]

        public virtual FeatureValue FeatureValue { get; set; }

        public long? EnableCategoryFeatureId { get; set; }

        public long? EnableValueId { get; set; }

        public string EnableFeatureValueCustomValue { get; set; }

        public long? DisableCategoryFeatureId { get; set; }

        public long? DisableValueId { get; set; }

        public string DisableFeatureValueCustomValue { get; set; }
    }
}
