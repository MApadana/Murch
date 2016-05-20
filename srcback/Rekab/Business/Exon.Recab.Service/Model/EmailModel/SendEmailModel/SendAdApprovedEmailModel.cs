using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.EmailModel.SendEmailModel
{
    public class SendAdApprovedEmailModel
    {
        public string FullName { get; set; }
        public string MailTo { get; set; }
        public string AdTitle { get; set; }
    }
}
