namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SMS", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SMS", "Type");
        }
    }
}
