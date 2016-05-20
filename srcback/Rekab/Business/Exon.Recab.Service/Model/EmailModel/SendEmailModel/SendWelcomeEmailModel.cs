using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.EmailModel.SendEmailModel
{
    public class SendWelcomeEmailModel
    {
        public string FullName { get; set; }
        public string UserDashboardLink { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string MailTo { get; set; }
    }
}
