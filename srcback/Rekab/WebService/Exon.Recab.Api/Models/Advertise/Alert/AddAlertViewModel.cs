using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Advertise.Alert
{
    public class AddAlertViewModel
    {
        public long userId { get; set; }

        public long categoryId { get; set; }

        public string title { get; set; }

        public bool sendEmail { get; set; }

        public bool sendSMS { get; set; }

        public bool sendPush { get; set; }

        public List<productSelectItemViewModel> alertItems { get; set; }
       
    }
}
