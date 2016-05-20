using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.UserModel
{
    public class UserProcuctDetailModel
    {
        public long Id { get; set; }

        public string adminComment { get; set; }
        public string confirmDate { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public string insertDate { get; set; }
        public List<string> mediaUrl { get; set; }
        public string tell { get; set; }

        public string userName { get; set; }

        public List<UserProductCFDetailViewModel> categoryFeature { get; set; }

        public UserProcuctDetailModel()
        {
            categoryFeature = new List<UserProductCFDetailViewModel>();
            mediaUrl = new List<string>();
        }

    }
}
