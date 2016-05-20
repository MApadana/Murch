using Exon.Recab.Api.Models.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.BaseAdmin
{
    public class DefaultFindModel : FindModel
    {
        public bool isEnable { get;  set; }
    }
}
