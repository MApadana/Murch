using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Infrastructure.Model
{
    public class ResponseWithCount : SimpleResponse
    {
        public long total { get; set; }
    }
}
