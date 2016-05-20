using System.Dynamic;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Authentication.Content.Models
{
    public class SingleRequestModel
    {
        [Required]
        public string url { get; set; }

        [Required]
        public string auth { get; set; }

        public string data { get; set; }

    }
}
