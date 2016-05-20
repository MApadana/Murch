using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Domain.Entity
{
    public class CategoryFeatureDependency : BaseEntity
    {
        public long CategoryFeatureId { get; set; }

        [ForeignKey("CategoryFeatureId")]
        public virtual CategoryFeature CategoryFeature { get; set; }

        public long? CategoryFeatureParentId { get; set; }

        [ForeignKey("CategoryFeatureParentId")]
        public virtual CategoryFeature CategoryFeatureParent { get; set; }

        public long? CategoryFeatureChildId { get; set; }

        [ForeignKey("CategoryFeatureChildId")]
        public virtual CategoryFeature CategoryFeatureChild { get; set; }

        public long? CategoryFeatureHideId { get; set; }

        [ForeignKey("CategoryFeatureHideId")]
        public virtual CategoryFeature CategoryFeatureHide { get; set; }

        public long? CategoryFeatureShowId { get; set; }

        [ForeignKey("CategoryFeatureShowId")]
        public virtual CategoryFeature CategoryFeatureShow { get; set; }


    }
}
