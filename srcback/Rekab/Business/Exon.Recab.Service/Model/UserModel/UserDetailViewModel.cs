using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exon.Recab.Domain.Constant.User;

namespace Exon.Recab.Service.Model.UserModel
{
    public class UserDetailViewModel
    {
        public string email { get;  set; }
        public string firstName { get;  set; }
        public int gender { get;  set; }
        public string lastName { get;  set; }
        public string mobile { get;  set; }
        public int status { get;  set; }
        public long cumUserId { get;  set; }
    }
}
