using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Authentication.Content.Models
{
    public class ResponseModel
    {
        public object result { get; set; }

        public int orderId { get; set; }
    }
}
