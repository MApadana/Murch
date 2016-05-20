using System.Collections.Generic;


namespace Exon.Recab.Domain.Entity.PackageModule
{
    public class PackageBaseConfig : BaseEntity
    {
        public string Title { get; set; }

        public string ValueType { get; set; }

        public virtual List<PurchaseConfig> PurchaseConfig { get; set; }

        public virtual List<PackageBaseConfigValue> PackageBaseConfigValue { get; set; }

        public PackageBaseConfig()
        {
            PurchaseConfig = new List<PurchaseConfig>();
            PackageBaseConfigValue = new List<PackageBaseConfigValue>();
        }
    }
}
