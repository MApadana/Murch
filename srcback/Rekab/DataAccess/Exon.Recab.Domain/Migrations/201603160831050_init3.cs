namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeatureValueDefaultValues", "EnableValueId", c => c.Long());
            AddColumn("dbo.FeatureValueDefaultValues", "DisableValueId", c => c.Long());
            DropColumn("dbo.FeatureValueDefaultValues", "FeatureValueEnableId");
            DropColumn("dbo.FeatureValueDefaultValues", "DisableValueEnableId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FeatureValueDefaultValues", "DisableValueEnableId", c => c.Long());
            AddColumn("dbo.FeatureValueDefaultValues", "FeatureValueEnableId", c => c.Long());
            DropColumn("dbo.FeatureValueDefaultValues", "DisableValueId");
            DropColumn("dbo.FeatureValueDefaultValues", "EnableValueId");
        }
    }
}
