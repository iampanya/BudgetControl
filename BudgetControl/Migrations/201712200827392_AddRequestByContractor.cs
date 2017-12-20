namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestByContractor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contractor",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        CostCenterID = c.String(maxLength: 10),
                        Name = c.String(maxLength: 255),
                        TitleName = c.String(maxLength: 100),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CostCenter", t => t.CostCenterID)
                .Index(t => t.CostCenterID);
            
            AddColumn("dbo.Payment", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Payment", "ContractorID", c => c.Guid());
            CreateIndex("dbo.Payment", "ContractorID");
            AddForeignKey("dbo.Payment", "ContractorID", "dbo.Contractor", "ID");

            Sql("UPDATE payment Set Type = 1");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payment", "ContractorID", "dbo.Contractor");
            DropForeignKey("dbo.Contractor", "CostCenterID", "dbo.CostCenter");
            DropIndex("dbo.Contractor", new[] { "CostCenterID" });
            DropIndex("dbo.Payment", new[] { "ContractorID" });
            DropColumn("dbo.Payment", "ContractorID");
            DropColumn("dbo.Payment", "Type");
            DropTable("dbo.Contractor");
        }
    }
}
