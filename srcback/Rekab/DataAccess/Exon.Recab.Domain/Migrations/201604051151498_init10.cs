namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init10 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Title", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
