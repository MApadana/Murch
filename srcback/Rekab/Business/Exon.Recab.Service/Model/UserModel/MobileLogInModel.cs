using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.UserModel
{
    public class MobileLogInModel
    {
        public string id { get; set; }

        public string firstName  { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public long credit { get;  set; }
        public LoginStatus status { get; internal set; }
        public bool isDealership { get;  set; }
        public bool activeDealership  { get; internal set; }
    }

    public enum LoginStatus
    {
     معمولی =0,
     اولین_بار_کاربر=1,
        اولین_بار_نمایشگاه=2


    }
}
