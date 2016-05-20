using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Advertise
{
    public class ADFineModel
    {
        public long userId { get; set; }

      
        public long advertiseId { get;  set; }

        public long? alertId { get; set; } 
       
    }
}
