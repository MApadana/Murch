using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.User
{
   public  class AddUserPackageCredit
    {
        public long userId { get; set; }

        public long packageTypeId { get; set; }
    }
}
