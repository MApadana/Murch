using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.EmailModel.SendEmailModel
{
    public class SendRegisteredAdReceiptEmailModel
    {
        public string FullName { get; set; }
        public string AdPage { get; set; }
        public string AdTitle { get; set; }
        public string Brand { get; set; }
        public string Used { get; set; }
        public string Year { get; set; }
        public string Provinces { get; set; }
        public string GearType { get; set; }
        public string Fuel { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Price { get; set; }
        public string State { get; set; }
        public string BodyState { get; set; }
        public string TagType { get; set; }
        public string MailTo { get; set; }
    }
}
