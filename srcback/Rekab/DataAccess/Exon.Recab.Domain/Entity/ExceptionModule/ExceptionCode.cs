using Exon.Recab.Domain.Constant.CS.Exception;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Domain.Entity.ExceptionModule
{
    public class ExceptionCode : BaseEntity
    {
        [Index(IsUnique = true)]        
        public ExceptionType ExceptionType { get; set; }

        
        [MaxLength(400)]
        public string Message { get; set; }
    }
}
