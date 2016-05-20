using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.SqlServer;
using System;
using System.Linq;
using Exon.Recab.Infrastructure.Exception;
using System.Net;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Service.Resource;

namespace Exon.Recab.Service.Implement.PolicySystemConfig
{
    public class ProductPolicyService
    {
        public RecabSystemConfig SystemConfig;

        public SdbContext _sdb;

        internal  ProductPolicyService(ref SdbContext sdb)
        {
            _sdb = sdb;

            SystemConfig = _sdb.RecabSystemConfig.First();

            if (SystemConfig == null)
                throw new RecabException("SQL Managment Error", HttpStatusCode.ExpectationFailed);
        }

        public ProductPolicyService()
        {
            _sdb = new SdbContext();

            SystemConfig = _sdb.RecabSystemConfig.First();

            if (SystemConfig == null)
                throw new RecabException("SQL Managment Error", HttpStatusCode.ExpectationFailed);
        }

        public void EnforcementInsert(ref Product product)
        {
            product.RaiseDate = DateTime.UtcNow.AddDays(-1);
            product.RaiseBaseQuota = 0;
            product.RaiseHourTime = 0;
            product.RaiseUsedQuota = product.RaiseBaseQuota;
            product.Priority = PriorityStatus.معمولی;

            //if (SystemConfig.AutomaticConfirmCreatedAD)
            //{
            //    product.Status = ProdoctStatus.فعال;
            //    product.ConfirmDate = DateTime.UtcNow;
            //}

        }

        public void EnforcementInsert(ref Product product, ref UserPackageCredit userPackage)
        {
            if (userPackage == null || userPackage.UsedQuota == 0)
                throw new RecabException("Package Quota unvalid", HttpStatusCode.BadRequest);

            userPackage.UsedQuota = userPackage.UsedQuota - 1;

            product.RaiseDate = DateTime.UtcNow.AddDays(-1);

            //PurchaseConfig HourConfig = userPackage.CategoryPurchasePackageType.PurchaseConfig.Find(pc => pc.PackageBaseConfig.Title == PackageConfig.RaiseHour);

            //if (HourConfig != null)
            //{
            //    int hour = 0;
            //    if (int.TryParse(HourConfig.Value, out hour))
            //    { product.RaiseHourTime = hour; }
            //    else
            //    {
            //        product.RaiseHourTime = 0;
            //    }
            //}
            //else
            //{
            //    product.RaiseHourTime = 0;
            //}

            //PurchaseConfig QuotaConfig = userPackage.CategoryPurchasePackageType.PurchaseConfig.Find(pc => pc.PackageBaseConfig.Title == PackageConfig.RaiseBaseQuota);


            //if (QuotaConfig != null)
            //{
            //    int quota = 0;
            //    if (int.TryParse(QuotaConfig.Value, out quota))
            //    {
            //        product.RaiseBaseQuota = quota;
            //    }
            //    else
            //    {
            //        product.RaiseBaseQuota = 0;
            //    }
            //}
            //else
            //{
            //    product.RaiseBaseQuota = 0;
            //}


            PurchaseConfig EditConfig = userPackage.CategoryPurchasePackageType.PurchaseConfig.Find(pc => pc.PackageBaseConfig.Title == PackageConfig.EditCount);


            if (EditConfig != null)
            {
                int quota = 0;
                if (int.TryParse(EditConfig.Value, out quota))
                {
                    product.UpdateCount = quota;
                }
                else
                {
                    product.UpdateCount = 0;
                }
            }
            else
            {
                product.UpdateCount = 0;
            }

            product.RaiseUsedQuota = product.RaiseBaseQuota;

            product.Priority = PriorityStatus.معمولی;

            if (SystemConfig.AutomaticConfirmCreatedAD)
            {
                product.Status = ProdoctStatus.فعال;
                product.ConfirmDate = DateTime.UtcNow;
            }

        }

        public void EnforcementUpdate(ref Product product)
        {

            if (SystemConfig.AutomaticConfirmUpdateAD)
                product.Status = ProdoctStatus.فعال;

        }

        public bool ValidateDefaultPictureCount(int picCount)
        {

            return picCount <= SystemConfig.ADSPictureInFirstTime;

        }

        public int GetAddefaultPictureCount()
        {
            return SystemConfig.ADSPictureInFirstTime;
        }

    }
}
