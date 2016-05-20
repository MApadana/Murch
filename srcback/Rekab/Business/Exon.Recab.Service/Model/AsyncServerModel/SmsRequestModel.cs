namespace Exon.Recab.Service.Model.AsyncServerModel
{
    public class SmsRequestModel
    {

        public SMSType type { get; set; }

        public string data { get; set; }
    }

    public enum SMSType
    {
        Simple = 5000,
        Multi = 5010,
        SendManager = 5020


    }
}
