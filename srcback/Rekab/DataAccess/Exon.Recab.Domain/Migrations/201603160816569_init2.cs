namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CategoryFeatureDefaultValues", "EnableCategoryFeatureId", c => c.Long());
            AlterColumn("dbo.CategoryFeatureDefaultValues", "FeatureValueEnableId", c => c.Long());
            AlterColumn("dbo.CategoryFeatureDefaultValues", "DisableCategoryFeatureId", c => c.Long());
            AlterColumn("dbo.CategoryFeatureDefaultValues", "DisableValueEnableId", c => c.Long());
            AlterColumn("dbo.FeatureValueDefaultValues", "EnableCategoryFeatureId", c => c.Long());
            AlterColumn("dbo.FeatureValueDefaultValues", "FeatureValueEnableId", c => c.Long());
            AlterColumn("dbo.FeatureValueDefaultValues", "DisableCategoryFeatureId", c => c.Long());
            AlterColumn("dbo.FeatureValueDefaultValues", "DisableValueEnableId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FeatureValueDefaultValues", "DisableValueEnableId", c => c.Long(nullable: false));
            AlterColumn("dbo.FeatureValueDefaultValues", "DisableCategoryFeatureId", c => c.Long(nullable: false));
            AlterColumn("dbo.FeatureValueDefaultValues", "FeatureValueEnableId", c => c.Long(nullable: false));
            AlterColumn("dbo.FeatureValueDefaultValues", "EnableCategoryFeatureId", c => c.Long(nullable: false));
            AlterColumn("dbo.CategoryFeatureDefaultValues", "DisableValueEnableId", c => c.Long(nullable: false));
            AlterColumn("dbo.CategoryFeatureDefaultValues", "DisableCategoryFeatureId", c => c.Long(nullable: false));
            AlterColumn("dbo.CategoryFeatureDefaultValues", "FeatureValueEnableId", c => c.Long(nullable: false));
            AlterColumn("dbo.CategoryFeatureDefaultValues", "EnableCategoryFeatureId", c => c.Long(nullable: false));
        }
    }
}
