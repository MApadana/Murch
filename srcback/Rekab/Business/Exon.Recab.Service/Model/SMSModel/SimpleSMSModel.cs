using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.SMSModel
{
    public class SimpleSMSModel
    {
        public string fromNumber { get; set; }

        public string toNumber { get; set; }

        public string content { get; set; }
    }
}
