using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Exon.Recab.Domain.Entity
{
    [Serializable]
    public class BaseEntity
    {
		[Key]
		[Column(Order = 0)]
        public long Id { get; set; }

    }
}
