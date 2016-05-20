using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity
{
    public class AndroidElement : BaseEntity
    {
       [MaxLength(50)]
       public string Type { get; set; }
 
    }
}
