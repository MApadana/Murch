using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.EmailModel.SendEmailModel
{
    public class SendReportAdEmailModel
    {
        public string FullName { get; set; }
        public string AdPage { get; set; }
        public string AdTitle { get; set; }
        public string WrongDescription { get; set; }
        public string WrongImage { get; set; }
        public string WrongPrice { get; set; }
        public string MailTo { get; set; }
    }
}
