using Exon.Recab.Api.Infrastructure.Resource;
using Exon.Recab.Api.Models.Advertise;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.AdminManageModel
{
    public class AdminEditAdvertiseModel
    {

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long cumUserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long advertiseId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public EditProductViewModel advertise { get; set; }

        public AddExchangePrpdoctMdoel exchange { get; set; }

        public AdminEditAdvertiseModel()
        {
            advertise = new EditProductViewModel();
            exchange = new AddExchangePrpdoctMdoel();
        }
    }
}
