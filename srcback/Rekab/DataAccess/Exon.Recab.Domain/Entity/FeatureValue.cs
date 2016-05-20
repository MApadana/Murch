using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
    public class FeatureValue : BaseEntity
    {
        [Required]
        [MaxLength(3000)]
        public string Title { get; set; }


        [MaxLength(3000)]
        public string Description { get; set; }


        [Required]
        public long CategoryFeatureId { get; set; }

        [ForeignKey("CategoryFeatureId")]
        public virtual CategoryFeature CategoryFeature { get; set; }

        public virtual List<FeatureValueDependency> ChildList { get; set; }

        public virtual List<FeatureValueDependency> ParentList { get; set; }

        public virtual List<FeatureValueDependency> HideList { get; set; }

        public virtual List<FeatureValueDependency> ShowList { get; set; }

        public virtual List<FeatureValueDefaultValue> DisableList { get; set; }

        public virtual List<FeatureValueDefaultValue> EnableList { get; set; }

        [MaxLength(3000)]
        public string ShowContainer { get; set; }

        [MaxLength(3000)]
        public string HideContainer { get; set; }

        public FeatureValue()
        {
            ChildList = new List<FeatureValueDependency>();
            ParentList = new List<FeatureValueDependency>();
            HideList = new List<FeatureValueDependency>();
            ShowList = new List<FeatureValueDependency>();
            DisableList = new List<FeatureValueDefaultValue>();
            EnableList = new List<FeatureValueDefaultValue>();
        }

    }
}
