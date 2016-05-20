using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class CategoryFeatureDefaultValue : BaseEntity
    {
        public long CategoryFeatureId { get; set; }

        [ForeignKey("CategoryFeatureId")]

        public virtual CategoryFeature CategoryFeature { get; set; }

        public long? EnableCategoryFeatureId { get; set; }

        public long? EnableFeatureValueId { get; set; }

        public string EnableFeatureValueCustomValue { get; set; }

        public long? DisableCategoryFeatureId { get; set; }

        public long? DisableFeatureValueId { get; set; }

        public string DisableFeatureValueCustomValue { get; set; }
    }
}
