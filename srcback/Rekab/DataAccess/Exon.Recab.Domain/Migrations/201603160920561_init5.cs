namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoryFeatureDependencies", "CategoryFeature_Id3", c => c.Long());
            CreateIndex("dbo.CategoryFeatureDependencies", "CategoryFeature_Id3");
            AddForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeature_Id3", "dbo.CategoryFeatures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeature_Id3", "dbo.CategoryFeatures");
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeature_Id3" });
            DropColumn("dbo.CategoryFeatureDependencies", "CategoryFeature_Id3");
        }
    }
}
