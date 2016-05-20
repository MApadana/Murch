using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Exon.Recab.CDN.Models
{
    public class HttpPostedFileModel
    {
        public HttpPostedFile data { get; set; }

        public long userId { get; set; }
    }
}
