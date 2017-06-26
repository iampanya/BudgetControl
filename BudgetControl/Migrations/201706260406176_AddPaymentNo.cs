namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentNo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentCounter",
                c => new
                    {
                        PaymentCounterID = c.Int(nullable: false, identity: true),
                        CostCenterID = c.String(maxLength: 10),
                        Year = c.String(maxLength: 4),
                        ShortCode = c.String(maxLength: 30),
                        Number = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.PaymentCounterID)
                .ForeignKey("dbo.CostCenter", t => t.CostCenterID)
                .Index(t => t.CostCenterID);
            
            AddColumn("dbo.Payment", "PaymentNo", c => c.String(maxLength: 35));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentCounter", "CostCenterID", "dbo.CostCenter");
            DropIndex("dbo.PaymentCounter", new[] { "CostCenterID" });
            DropColumn("dbo.Payment", "PaymentNo");
            DropTable("dbo.PaymentCounter");
        }
    }
}
