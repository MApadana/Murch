using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Entity.PackageModule;

namespace Exon.Recab.Domain.Entity
{
    public class Product : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public long? DealershipId { get; set; }

        [ForeignKey("DealershipId")]
        public virtual Dealership Dealership { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public long? UserPackageCreditId { get; set; }

        [ForeignKey("UserPackageCreditId")]
        public virtual UserPackageCredit UserPackageCredit { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public ProdoctStatus Status { get; set; }

        [Required]
        public DateTime InsertDate { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        [Required]
        public int UpdateCount { get; set; }

        [Required]
        public PriorityStatus Priority { get; set; }

        [Required]
        public DateTime RaiseDate { get; set; }

        [Required]
        public int RaiseBaseQuota { get; set; }

        [Required]
        public int RaiseUsedQuota { get; set; }

        [Required]
        public int RaiseHourTime { get; set; }

        public long WebVisitCount { get; set; }

        public long AndroidVisitCount { get; set; }

        public long IosVisitCount { get; set; }

        [MaxLength(100)]
        public string Tell { get; set; }

        public long? CategoryExchangeId { get; set; }

        public virtual List<ProductFeatureValue> ProductFeatures { get; set; }

        public virtual List<ExchangeFeatureValue> ExchangeFeatureValues { get; set; }

        [MaxLength(500)]
        public string AdminComment { get; set; }


        public Product()
        {
            ProductFeatures = new List<ProductFeatureValue>();

            ExchangeFeatureValues = new List<Entity.ExchangeFeatureValue>();

        }

    }
}
