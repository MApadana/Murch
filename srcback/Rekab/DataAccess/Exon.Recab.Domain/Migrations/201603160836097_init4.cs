namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoryFeatureDefaultValues", "EnableFeatureValueId", c => c.Long());
            AddColumn("dbo.CategoryFeatureDefaultValues", "DisableFeatureValueId", c => c.Long());
            DropColumn("dbo.CategoryFeatureDefaultValues", "FeatureValueEnableId");
            DropColumn("dbo.CategoryFeatureDefaultValues", "DisableValueEnableId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CategoryFeatureDefaultValues", "DisableValueEnableId", c => c.Long());
            AddColumn("dbo.CategoryFeatureDefaultValues", "FeatureValueEnableId", c => c.Long());
            DropColumn("dbo.CategoryFeatureDefaultValues", "DisableFeatureValueId");
            DropColumn("dbo.CategoryFeatureDefaultValues", "EnableFeatureValueId");
        }
    }
}
