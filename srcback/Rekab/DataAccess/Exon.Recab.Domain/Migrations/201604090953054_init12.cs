namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoryFeatures", "RequierdInTPInsert", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CategoryFeatures", "RequierdInTPInsert");
        }
    }
}
