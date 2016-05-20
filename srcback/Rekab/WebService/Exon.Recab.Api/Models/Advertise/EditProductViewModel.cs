using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Advertise
{
    public class EditProductViewModel
    {

        public long? dealershipId { get; set; }

        public List<AddAdvertiseMediaModel> media { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string tell { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string description { get; set; }

        //rename2 :  advertiseItems
        public List<productSelectItemViewModel> advertiseItems { get; set; }

        public EditProductViewModel()
        {
            advertiseItems = new List<productSelectItemViewModel>();

            media = new List<AddAdvertiseMediaModel>();
        }
    }
}
