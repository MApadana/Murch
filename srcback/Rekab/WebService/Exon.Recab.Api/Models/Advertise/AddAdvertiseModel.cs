using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.Advertise
{
    public class AddAdvertiseModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public AddProductViewModel advertise { get; set; }

        public AddExchangePrpdoctMdoel exchange { get; set; }

        public AddAdvertiseModel()
        {
            advertise = new AddProductViewModel();
            exchange = new AddExchangePrpdoctMdoel();
        }
    }
}
