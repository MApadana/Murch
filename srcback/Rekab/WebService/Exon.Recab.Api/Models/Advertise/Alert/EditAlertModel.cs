using System;
using System.Collections.Generic;

namespace Exon.Recab.Api.Models.Advertise.Alert
{
    public class EditAlertModel
    {
        public long alertId { get;  set; }

        public List<productSelectItemViewModel> alertItems { get; set; }

        public string title { get;  set; }

        public long userId { get; set; }

        public bool sendEmail { get;  set; }

        public bool sendSMS { get;  set; }

        public bool sendPush { get; set; }

        public EditAlertModel()
        {
            alertItems = new List<productSelectItemViewModel>();
        }

    }
}
