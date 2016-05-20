using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Domain.Entity
{
    public class State :BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<City> Cities { get; set; }

        public State()
        {
            Cities = new List<City>(); 
        }
    }
}
