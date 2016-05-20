using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Model.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Implement.Admin
{
    public class AdminUserService
    {
        private readonly SdbContext _sdb;

        public AdminUserService()
        {
            _sdb = new SdbContext();
        }

        public List<PackageBuyAdminDetail> PackageBuyAdminDetail(string fromPersianData, string toPersianData, ref long count, int size = 1, int skip = 0)
        {
            List<Credit> Credits = _sdb.Credits.Where(c => c.UserPackageCreditId.HasValue).ToList();


            if (fromPersianData != null && fromPersianData != "")
            {
                DateTime From = fromPersianData.PersianToGregorianUTC();

                Credits = Credits.Where(c => c.InsertTime > From).ToList();
            }

            if (toPersianData != null && toPersianData != "")
            {
                DateTime To = toPersianData.PersianToGregorianUTC();

                Credits = Credits.Where(c => c.InsertTime < To).ToList();
            }

            count = Credits.Count;
            if (count == 0)
                return new List<PackageBuyAdminDetail>();

            return Credits.Select(c => new PackageBuyAdminDetail
            {
                creditId = c.Id,
                usreName = c.User.FirstName ?? "" + " " + c.User.LastName ?? "",
                userId = c.UserId,
                userMobile = c.User.Mobile ?? "",
                totalCount = c.UserPackageCredit.BaseQuota,
                usedCount = c.UserPackageCredit.BaseQuota - c.UserPackageCredit.UsedQuota,
                unUsedCount = c.UserPackageCredit.UsedQuota,
                date = c.InsertTime.UTCToPersianDateLong(),
                status = c.UserPackageCredit.Status,
                statusTitle = c.UserPackageCredit.Status.ToString(),
                cpptId = c.UserPackageCredit.CategoryPurchasePackageTypeId,
                purchaseType = c.UserPackageCredit.CategoryPurchasePackageType.CategoryPurchaseType.PurchaseType.Title,
                category = c.UserPackageCredit.CategoryPurchasePackageType.CategoryPurchaseType.Category.Title,
                packageType = c.UserPackageCredit.CategoryPurchasePackageType.PackageType.Title,
                upcId = c.UserPackageCreditId.Value
            }).OrderByDescending(c => c.creditId)
              .Skip(size * skip)
              .Take(size)
              .ToList();
        }

        public bool ChangeUserPackageCreditStatus(long upcId, int status)
        {
            UserPackageCredit userCredit = _sdb.UserPackageCredits.Find(upcId);
            if (userCredit == null)
                throw new RecabException((int)ExceptionType.UserPackageCreditNotFound);

            userCredit.Status = (UserCreditStatus)status;

            _sdb.SaveChanges();

            return true;

        }
    }
}
