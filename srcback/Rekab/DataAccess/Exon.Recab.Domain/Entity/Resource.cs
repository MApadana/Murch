using Exon.Recab.Domain.Constant.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Domain.Entity
{
    public class Resource : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Url { get; set; }

        [Required]
        public ResourceType Type { get; set; }

        public long? ParentId { get; set; }

        [Required]
        [MaxLength(15)]
        public string Key { get; set; }


        public virtual List<Permission> Permission { get; set; }

        public Resource()
        {
            Permission = new List<Permission>();
        }

    }
}
