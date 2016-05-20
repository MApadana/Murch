using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Domain.Entity
{
    public class Element : BaseEntity
    { 
        [Required]
        [MaxLength(15)]
        public string Title { get; set; }

        [MaxLength(15)]
        public string HtmlId { get; set; }

        [MaxLength(15)]
        public string HtmlName { get; set; }

        [MaxLength(50)]
        public string DefaulteClass { get; set; }
          

    }
}
