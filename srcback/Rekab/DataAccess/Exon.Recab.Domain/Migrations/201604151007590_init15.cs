namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecabSystemConfigs", "NewUesrVoucher", c => c.Int(nullable: false));
            AddColumn("dbo.RecabSystemConfigs", "NewDealershipVoucher", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecabSystemConfigs", "NewDealershipVoucher");
            DropColumn("dbo.RecabSystemConfigs", "NewUesrVoucher");
        }
    }
}
