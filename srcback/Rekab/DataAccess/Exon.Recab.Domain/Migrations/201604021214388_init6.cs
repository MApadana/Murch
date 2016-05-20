namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ReletiveCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ReletiveCount");
        }
    }
}
