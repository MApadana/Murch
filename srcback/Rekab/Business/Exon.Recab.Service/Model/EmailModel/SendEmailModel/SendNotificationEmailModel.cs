using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.EmailModel.SendEmailModel
{
    public class SendNotificationEmailModel
    {
        public string MailTo { get; set; }
        public IEnumerable<NotificationAdEmailModel> ListOfAds { get; set; }
        public string FullName { get; set; }
        public string AdLink { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Price { get; set; }
        public string Used { get; set; }
        public string SalesType { get; set; }
    }
}
