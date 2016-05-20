namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AlertProducts", "Title", c => c.String(nullable: false, maxLength: 3000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AlertProducts", "Title", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
