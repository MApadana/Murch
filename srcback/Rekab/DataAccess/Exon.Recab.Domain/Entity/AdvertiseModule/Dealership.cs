using System.ComponentModel.DataAnnotations;
using Exon.Recab.Domain.Constant.User;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Exon.Recab.Domain.Entity
{
    public class Dealership : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]

        public virtual User User { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public DealershipStatus Status { get; set; }

        [Required]
        [MaxLength(400)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tell { get; set; }

        [MaxLength(20)]
        public string Fax { get; set; }

        public double CoordinateLat { get; set; }

        public double CoordinateLong { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string WebsiteUrl { get; set; }

        [MaxLength(500)]
        public string LogoUrl { get; set; }

        [Required]
        public DateTime InsertDate { get; set; }

        
        public DateTime? ConfirmDate { get; set; }

        [Required]
        public long CityId { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        public virtual List<DealershipCategory> DealershipCategory { get; set; }

        public virtual List<Product> ProductList { get; set; }

        public Dealership()
        {
            ProductList = new List<Product>();
            DealershipCategory = new List<DealershipCategory>();
        }


    }
}
