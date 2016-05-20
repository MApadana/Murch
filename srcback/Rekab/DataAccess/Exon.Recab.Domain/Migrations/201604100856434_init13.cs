namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlertProducts", "Description", c => c.String(nullable: false, maxLength: 3000));
            AddColumn("dbo.AlertProducts", "SendPush", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AlertProducts", "Title", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AlertProducts", "Title", c => c.String(nullable: false, maxLength: 3000));
            DropColumn("dbo.AlertProducts", "SendPush");
            DropColumn("dbo.AlertProducts", "Description");
        }
    }
}
