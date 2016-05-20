using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Advertise
{
   public class AddAdvertiseMediaModel
    {
        public string url { get; set; }

        public DataType type { get; set; }

        public int orderId { get; set; }
    }

    public enum DataType
    {
        Picture = 0,
        Video =1

    }
}
