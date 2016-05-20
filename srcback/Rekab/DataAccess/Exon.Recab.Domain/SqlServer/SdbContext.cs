using System.Data.Entity;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Domain.Entity.CMS;
using Exon.Recab.Domain.Entity.AlertModule;
using Exon.Recab.Domain.Entity.ExceptionModule;
using Exon.Recab.Domain.Entity.BackUpModole;

namespace Exon.Recab.Domain.SqlServer
{
    public class SdbContext : DbContext
    {
        static SdbContext()
        {
            Database.SetInitializer<SdbContext>(null);
        }

        public SdbContext() : base("name=RecabDataContext")
        { }

        public virtual DbSet<Category> Categoris { get; set; }

        public virtual DbSet<CategoryFeature> CategoryFeatures { get; set; }

        public virtual DbSet<Element> Elements { get; set; }

        public virtual DbSet<FeatureValue> FeatureValues { get; set; }

        public virtual DbSet<CategoryFeatureDependency> CategoryFeatureDependencies { get; set; }

        public virtual DbSet<FeatureValueDependency> FeatureValueDependencies { get; set; }

        public virtual DbSet<AndroidElement> AndroidElements { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<ProductFeatureValue> ProductFeatureValues { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserToken> UserTokens { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<PackageBaseConfig> PackageBaseConfig { get; set; }

        public virtual DbSet<PackageBaseConfigValue> PackageBaseConfigValues { get; set; }

        public virtual DbSet<PurchaseConfig> PurchaseConfig { get; set; }

        public virtual DbSet<PackageType> PackageTypes { get; set; }

        public virtual DbSet<PurchaseType> PurchaseType { get; set; }

        public virtual DbSet<CategoryPurchaseType> CategoryPurchaseTypes { get; set; }

        public virtual DbSet<CategoryPurchasePackageType> CategoryPurchasePackageTypes { get; set; }

        public virtual DbSet<UserPackageCredit> UserPackageCredits { get; set; }

        public virtual DbSet<Dealership> Dealerships { get; set; }

        public virtual DbSet<FeatureValueAssign> FeatureValueAssign { get; set; }

        public virtual DbSet<SMS> SMS { get; set; }

        public virtual DbSet<State> State { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<DealershipCategory> DealershipCategory { get; set; }

        public virtual DbSet<CategoryExchange> CategoryExchange { get; set; }

        public virtual DbSet<Media> Media { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }

        public virtual DbSet<Permission> Permission { get; set; }

        public virtual DbSet<Credit> Credits { get; set; }

        public virtual DbSet<ExchangeFeatureValue> ExchangeFeatureValues { get; set; }

        public virtual DbSet<ProductFeatureValueFeatureValueItem> ProductFeatureValueFeatureValueItems { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<RecabSystemConfig> RecabSystemConfig { get; set; }

        public virtual DbSet<FeatureValueAssignFeatureValueItem> FeatureValueAssignFeatureValueItems { get; set; }

        public virtual DbSet<FavouriteProduct> FavouriteProduct { get; set; }

        public virtual DbSet<FeedbackProduct> FeedbackProduct { get; set; }

        public virtual DbSet<Article> Articles { get; set; }

        public virtual DbSet<ArticleStructure> ArticleStructures { get; set; }

        public virtual DbSet<TodayPriceConfig> TodayPricesConfig { get; set; }

        public virtual DbSet<TodayPriceOption> TodayPriceOption { get; set; }

        public virtual DbSet<Voucher> Voucher { get; set; }

        public virtual DbSet<VoucherConfig> VoucherConfig { get; set; }

        public virtual DbSet<AlertProduct> AlertProduct { get; set; }

        public virtual DbSet<ExceptionCode> ExceptionCodes { get; set; }

        public virtual DbSet<MongoBackUp> MongoBackUps { get; set; }

        public virtual DbSet<FeatureValueDefaultValue> FeatureValueDefaultValues { get; set; }

        public virtual DbSet<CategoryFeatureDefaultValue> CategoryFeatureDefaultValues { get; set; }

    }
}



