namespace Exon.Recab.CDN.Models
{
    public class Base64InputModel
    {
        public string data { get; set; }

        public bool sizeSensitive { get; set; }

        public bool waterMark { get; set; }

        public string extension { get; set; }

        public long userId { get; set; }
    }
}
