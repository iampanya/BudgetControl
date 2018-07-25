namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UploadTempBudget : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempBudget",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountID = c.String(maxLength: 8),
                        AccountName = c.String(maxLength: 255),
                        CostCenterID = c.String(maxLength: 10),
                        Year = c.String(maxLength: 4),
                        BudgetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UploadBy = c.String(maxLength: 50),
                        UploadTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempBudget");
        }
    }
}
