using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.AdminManageModel
{
    public class ChangeUserPackageCreditStatusModel
    {
        public long userPackageCreditId { get;  set; }

        public UserPackageCreditStatus status { get; set; }
    }
    public enum UserPackageCreditStatus
    {
     فعال=0,
     غیرفعال=1

    }
}
