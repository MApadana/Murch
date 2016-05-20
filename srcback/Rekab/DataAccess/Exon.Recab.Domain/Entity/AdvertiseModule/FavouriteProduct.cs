using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Exon.Recab.Domain.Entity
{
    public class FavouriteProduct :BaseEntity
    {
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }
}
