namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountID = c.String(nullable: false, maxLength: 10),
                        AccountName = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccountID);
            
            CreateTable(
                "dbo.Budget",
                c => new
                    {
                        BudgetID = c.Guid(nullable: false),
                        AccountID = c.String(maxLength: 10),
                        CostCenterID = c.String(maxLength: 10),
                        Sequence = c.Int(nullable: false),
                        Year = c.String(maxLength: 4),
                        BudgetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WithdrawAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.BudgetID)
                .ForeignKey("dbo.Account", t => t.AccountID)
                .ForeignKey("dbo.CostCenter", t => t.CostCenterID)
                .Index(t => t.AccountID)
                .Index(t => t.CostCenterID);
            
            CreateTable(
                "dbo.BudgetTransaction",
                c => new
                    {
                        BudgetTransactionID = c.Guid(nullable: false),
                        BudgetID = c.Guid(nullable: false),
                        PaymentID = c.Guid(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Description = c.String(maxLength: 255),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreviousAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefID = c.Guid(),
                        Type = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.BudgetTransactionID)
                .ForeignKey("dbo.Budget", t => t.BudgetID, cascadeDelete: true)
                .ForeignKey("dbo.Payment", t => t.PaymentID)
                .ForeignKey("dbo.BudgetTransaction", t => t.RefID)
                .Index(t => t.BudgetID)
                .Index(t => t.PaymentID)
                .Index(t => t.RefID);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        PaymentID = c.Guid(nullable: false),
                        CostCenterID = c.String(maxLength: 10),
                        Year = c.String(maxLength: 4),
                        Sequence = c.Int(nullable: false),
                        Description = c.String(maxLength: 255),
                        RequestBy = c.String(maxLength: 10),
                        PaymentDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ControlBy = c.String(maxLength: 10),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.PaymentID)
                .ForeignKey("dbo.CostCenter", t => t.CostCenterID)
                .ForeignKey("dbo.Employee", t => t.ControlBy)
                .ForeignKey("dbo.Employee", t => t.RequestBy)
                .Index(t => t.CostCenterID)
                .Index(t => t.RequestBy)
                .Index(t => t.ControlBy);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeID = c.String(nullable: false, maxLength: 10),
                        TitleName = c.String(maxLength: 64),
                        FirstName = c.String(maxLength: 128),
                        LastName = c.String(maxLength: 128),
                        JobTitle = c.String(maxLength: 128),
                        JobLevel = c.Byte(nullable: false),
                        CostCenterID = c.String(maxLength: 10),
                        Password = c.String(maxLength: 32),
                        Role = c.String(maxLength: 32),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.CostCenter", t => t.CostCenterID)
                .Index(t => t.CostCenterID);
            
            CreateTable(
                "dbo.CostCenter",
                c => new
                    {
                        CostCenterID = c.String(nullable: false, maxLength: 10),
                        CostCenterName = c.String(maxLength: 128),
                        ShortName = c.String(maxLength: 64),
                        LongName = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.CostCenterID);
            
            CreateTable(
                "dbo.Statement",
                c => new
                    {
                        StatementID = c.Guid(nullable: false),
                        PaymentID = c.Guid(nullable: false),
                        BudgetID = c.Guid(nullable: false),
                        Description = c.String(),
                        WithdrawAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreviousAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.StatementID)
                .ForeignKey("dbo.Budget", t => t.BudgetID, cascadeDelete: true)
                .ForeignKey("dbo.Payment", t => t.PaymentID, cascadeDelete: true)
                .Index(t => t.PaymentID)
                .Index(t => t.BudgetID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.String(maxLength: 10),
                        UserName = c.String(maxLength: 10),
                        Password = c.String(maxLength: 32),
                        Token = c.Guid(),
                        ExpireDate = c.DateTime(),
                        Role = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        LastLogin = c.DateTime(),
                        Remark = c.String(maxLength: 256),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID)
                .Index(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.BudgetTransaction", "RefID", "dbo.BudgetTransaction");
            DropForeignKey("dbo.Statement", "PaymentID", "dbo.Payment");
            DropForeignKey("dbo.Statement", "BudgetID", "dbo.Budget");
            DropForeignKey("dbo.Payment", "RequestBy", "dbo.Employee");
            DropForeignKey("dbo.Payment", "ControlBy", "dbo.Employee");
            DropForeignKey("dbo.Payment", "CostCenterID", "dbo.CostCenter");
            DropForeignKey("dbo.Employee", "CostCenterID", "dbo.CostCenter");
            DropForeignKey("dbo.Budget", "CostCenterID", "dbo.CostCenter");
            DropForeignKey("dbo.BudgetTransaction", "PaymentID", "dbo.Payment");
            DropForeignKey("dbo.BudgetTransaction", "BudgetID", "dbo.Budget");
            DropForeignKey("dbo.Budget", "AccountID", "dbo.Account");
            DropIndex("dbo.User", new[] { "EmployeeID" });
            DropIndex("dbo.Statement", new[] { "BudgetID" });
            DropIndex("dbo.Statement", new[] { "PaymentID" });
            DropIndex("dbo.Employee", new[] { "CostCenterID" });
            DropIndex("dbo.Payment", new[] { "ControlBy" });
            DropIndex("dbo.Payment", new[] { "RequestBy" });
            DropIndex("dbo.Payment", new[] { "CostCenterID" });
            DropIndex("dbo.BudgetTransaction", new[] { "RefID" });
            DropIndex("dbo.BudgetTransaction", new[] { "PaymentID" });
            DropIndex("dbo.BudgetTransaction", new[] { "BudgetID" });
            DropIndex("dbo.Budget", new[] { "CostCenterID" });
            DropIndex("dbo.Budget", new[] { "AccountID" });
            DropTable("dbo.User");
            DropTable("dbo.Statement");
            DropTable("dbo.CostCenter");
            DropTable("dbo.Employee");
            DropTable("dbo.Payment");
            DropTable("dbo.BudgetTransaction");
            DropTable("dbo.Budget");
            DropTable("dbo.Account");
        }
    }
}
