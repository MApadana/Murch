using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.News
{
    public class AddEmailSubscribeModel
    {
        [EmailAddress]
        public string email { get; set; }
    }
}