using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.Advertise
{
    public class AddProductViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        public long? dealershipId { get; set; }

        public List<AddAdvertiseMediaModel> media { get; set; }
        
        public long? packageId { get; set; }

        public long? categoryPurchasePackageTypeId { get; set; } 

        public string voucherCode { get; set; }

        public PaymentType paymentType { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string tell { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string description { get; set; }

        //rename2 :            advertiseItems
        public List<productSelectItemViewModel> advertiseItems { get; set; }

        public AddProductViewModel()
        {
            advertiseItems = new List<productSelectItemViewModel>();

            media = new List<AddAdvertiseMediaModel>();
        }
    }

    public enum PaymentType
    {
        بانک_ملت=0,
        کیف_پول=1

    }
}
