using Exon.Recab.Domain.Constant.Media;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity
{
    public class Media : BaseEntity
    {
        [Required]
        public string MediaURL { get; set; }
        
        [Required]
        public long EntityId { get; set; }

        public int Order { get; set; }

        [Required]
        public MediaType MediaType { get; set; }

        [Required]
        public EntityType EntityType { get; set; }

    }
}
