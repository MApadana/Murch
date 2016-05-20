namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "MobileVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "EmailVerified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "EmailVerified");
            DropColumn("dbo.Users", "MobileVerified");
        }
    }
}
