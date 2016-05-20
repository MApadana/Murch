using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.UserModel
{
    public class UserDetailAdminViewModel
    {
        public long credit { get;  set; }
        public long cumUserId { get;  set; }
        public string email { get;  set; }
        public string mobile { get;  set; }
        public string name { get;  set; }
        public int status { get;  set; }
    }
}
