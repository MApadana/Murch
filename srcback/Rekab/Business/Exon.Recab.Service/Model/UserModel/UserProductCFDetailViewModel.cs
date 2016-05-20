using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.UserModel
{
    public class UserProductCFDetailViewModel
    {
        public string title { get; set; }

        public long categoryfeatureId { get; set; }

        public List<UserProductFVDetailViewModel> featureValue { get; set; }
        public string customValue { get; set; }

        public UserProductCFDetailViewModel()
        {
            featureValue = new List<UserProductFVDetailViewModel>();
        }

    }
}
