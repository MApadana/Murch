using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exon.Recab.Domain.Constant.User;

namespace Exon.Recab.Service.Model.AdminModel
{
    public class PackageBuyAdminDetail
    {
        public string category { get; set; }
        public long cpptId { get; set; }
        public long creditId { get; set; }
        public string date { get; set; }
        public string packageType { get; set; }
        public string purchaseType { get; set; }
        public UserCreditStatus status { get; set; }
        public string statusTitle { get; set; }
        public long totalCount { get; set; }
        public long unUsedCount { get; set; }
        public long upcId { get; set; }
        public long usedCount { get; set; }
        public long userId { get; set; }
        public string userMobile { get; set; }
        public string usreName { get; set; }
    }
}
