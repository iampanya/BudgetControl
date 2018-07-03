namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrentWorkingCC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CurrentWorkingCC",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EmployeeID = c.String(maxLength: 10),
                        WorkingCostCenterID = c.String(maxLength: 10),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                        DeletedBy = c.String(maxLength: 128),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.DeletedAt);
            
            AddColumn("dbo.WorkingCC", "CostCenterCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.WorkingCC", "CostCenterCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkingCC", new[] { "CostCenterCode" });
            DropIndex("dbo.CurrentWorkingCC", new[] { "DeletedAt" });
            DropColumn("dbo.WorkingCC", "CostCenterCode");
            DropTable("dbo.CurrentWorkingCC");
        }
    }
}
