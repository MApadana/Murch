using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Role
{
    public class UserChangeStatusModel
    {
        public long cumUserId { get; set; }

        public int status { get; set; }

    }
}
