using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Exon.Recab.Domain.Entity
{
    public class City : BaseEntity
    {
        [Required]
        public long StateId { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
        

    }
}
