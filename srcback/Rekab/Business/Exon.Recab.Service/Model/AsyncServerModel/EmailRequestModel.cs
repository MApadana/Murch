namespace Exon.Recab.Service.Model.AsyncServerModel
{
    public class EmailRequestModel
    {
        public EmailType type { get; set; }

        public string data { get; set; }
    }

    public enum EmailType
    {
        MandrillSimple = 4000,
        MandrillAttateched = 4010,
        MandrillBulk = 4020

    }
}
