using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Infrastructure.Model
{
    public class SimpleResponse
    {
        public object data { get; set; }

        public string message { get; set; }

        public int status { get; set; }

        public int exceptionCode { get; set; }
        public List<object> modelStateError { get; set; }
    }
}
