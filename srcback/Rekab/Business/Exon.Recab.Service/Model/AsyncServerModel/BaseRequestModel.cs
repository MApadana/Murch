using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.AsyncServerModel
{
   public class BaseRequestModel
    {
        [Required]
        public string url { get; set; }

        public string data { get; set; }
    }
}
