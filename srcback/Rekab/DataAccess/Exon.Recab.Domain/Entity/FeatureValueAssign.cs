using Exon.Recab.Domain.Constant.Media;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Exon.Recab.Domain.Entity
{
	public class FeatureValueAssign : BaseEntity
	{		
        [Required]
        public long EntityId { get; set; }

        [Required]
        public EntityType EntityType { get; set; }

        [Required]
        public long CategoryFeatureId { get; set; }

		[ForeignKey("CategoryFeatureId")]
		public virtual CategoryFeature CategoryFeature { get; set; }

        public virtual List<FeatureValueAssignFeatureValueItem> ListFeatureValue { get; set; }

        [MaxLength(100)]
        public string CustomValue { get; set; }


        public FeatureValueAssign()
        {
            ListFeatureValue = new List<FeatureValueAssignFeatureValueItem>();
        }        

    }
}
