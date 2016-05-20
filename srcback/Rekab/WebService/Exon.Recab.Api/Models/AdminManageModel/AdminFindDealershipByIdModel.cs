using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.AdminManageModel
{
    public class AdminFindDealershipByIdModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long dealershipid { get; set; }
    }
}
