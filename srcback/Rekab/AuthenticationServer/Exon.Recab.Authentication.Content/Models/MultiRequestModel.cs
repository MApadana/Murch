using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Authentication.Content.Models
{
    public class MultiRequestModel
    {
        [Required]
        public int orderId { get; set; }

        [Required]
        public string url { get; set; }

        [Required]
        public string auth { get; set; }

        public string data { get; set; }

    }
}
