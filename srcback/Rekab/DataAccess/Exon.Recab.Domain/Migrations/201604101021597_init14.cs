namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AlertFeatureValues", "AlertProductId", "dbo.AlertProducts");
            DropForeignKey("dbo.AlertFeatureValues", "CategoryFeatureId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.AlertFeatureValueFeatureValueItems", "AlertFeatureValueId", "dbo.AlertFeatureValues");
            DropForeignKey("dbo.AlertFeatureValueFeatureValueItems", "FeatureValueId", "dbo.FeatureValues");
            DropIndex("dbo.AlertFeatureValues", new[] { "AlertProductId" });
            DropIndex("dbo.AlertFeatureValues", new[] { "CategoryFeatureId" });
            DropIndex("dbo.AlertFeatureValueFeatureValueItems", new[] { "AlertFeatureValueId" });
            DropIndex("dbo.AlertFeatureValueFeatureValueItems", new[] { "FeatureValueId" });
            DropTable("dbo.AlertFeatureValues");
            DropTable("dbo.AlertFeatureValueFeatureValueItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AlertFeatureValueFeatureValueItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AlertFeatureValueId = c.Long(nullable: false),
                        FeatureValueId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AlertFeatureValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AlertProductId = c.Long(nullable: false),
                        CategoryFeatureId = c.Long(nullable: false),
                        CustomValue = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.AlertFeatureValueFeatureValueItems", "FeatureValueId");
            CreateIndex("dbo.AlertFeatureValueFeatureValueItems", "AlertFeatureValueId");
            CreateIndex("dbo.AlertFeatureValues", "CategoryFeatureId");
            CreateIndex("dbo.AlertFeatureValues", "AlertProductId");
            AddForeignKey("dbo.AlertFeatureValueFeatureValueItems", "FeatureValueId", "dbo.FeatureValues", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AlertFeatureValueFeatureValueItems", "AlertFeatureValueId", "dbo.AlertFeatureValues", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AlertFeatureValues", "CategoryFeatureId", "dbo.CategoryFeatures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AlertFeatureValues", "AlertProductId", "dbo.AlertProducts", "Id", cascadeDelete: true);
        }
    }
}
