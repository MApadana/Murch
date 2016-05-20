namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryFeatureDefaultValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryFeatureId = c.Long(nullable: false),
                        EnableCategoryFeatureId = c.Long(nullable: false),
                        FeatureValueEnableId = c.Long(nullable: false),
                        EnableFeatureValueCustomValue = c.String(),
                        DisableCategoryFeatureId = c.Long(nullable: false),
                        DisableValueEnableId = c.Long(nullable: false),
                        DisableFeatureValueCustomValue = c.String(),
                        CategoryFeature_Id = c.Long(),
                        CategoryFeature_Id1 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureId, cascadeDelete: false)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeature_Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeature_Id1)
                .Index(t => t.CategoryFeatureId)
                .Index(t => t.CategoryFeature_Id)
                .Index(t => t.CategoryFeature_Id1);
            
            CreateTable(
                "dbo.FeatureValueDefaultValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FeatureValueId = c.Long(nullable: false),
                        EnableCategoryFeatureId = c.Long(nullable: false),
                        FeatureValueEnableId = c.Long(nullable: false),
                        EnableFeatureValueCustomValue = c.String(),
                        DisableCategoryFeatureId = c.Long(nullable: false),
                        DisableValueEnableId = c.Long(nullable: false),
                        DisableFeatureValueCustomValue = c.String(),
                        FeatureValue_Id = c.Long(),
                        FeatureValue_Id1 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValueId, cascadeDelete: false)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValue_Id)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValue_Id1)
                .Index(t => t.FeatureValueId)
                .Index(t => t.FeatureValue_Id)
                .Index(t => t.FeatureValue_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeatureValueDefaultValues", "FeatureValue_Id1", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDefaultValues", "FeatureValue_Id", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDefaultValues", "FeatureValueId", "dbo.FeatureValues");
            DropForeignKey("dbo.CategoryFeatureDefaultValues", "CategoryFeature_Id1", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatureDefaultValues", "CategoryFeature_Id", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatureDefaultValues", "CategoryFeatureId", "dbo.CategoryFeatures");
            DropIndex("dbo.FeatureValueDefaultValues", new[] { "FeatureValue_Id1" });
            DropIndex("dbo.FeatureValueDefaultValues", new[] { "FeatureValue_Id" });
            DropIndex("dbo.FeatureValueDefaultValues", new[] { "FeatureValueId" });
            DropIndex("dbo.CategoryFeatureDefaultValues", new[] { "CategoryFeature_Id1" });
            DropIndex("dbo.CategoryFeatureDefaultValues", new[] { "CategoryFeature_Id" });
            DropIndex("dbo.CategoryFeatureDefaultValues", new[] { "CategoryFeatureId" });
            DropTable("dbo.FeatureValueDefaultValues");
            DropTable("dbo.CategoryFeatureDefaultValues");
        }
    }
}
