namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreEmployeeInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessAreaInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BaCode = c.String(maxLength: 4),
                        BaName = c.String(maxLength: 200),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.BaCode, unique: true);
            
            CreateTable(
                "dbo.DepartmentInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeptSap = c.Int(nullable: false),
                        DeptUpper = c.Int(nullable: false),
                        DeptStableCode = c.String(maxLength: 6),
                        DeptChangeCode = c.String(maxLength: 15),
                        DeptOldCode = c.String(maxLength: 15),
                        DeptShort = c.String(maxLength: 100),
                        DeptShort1 = c.String(maxLength: 100),
                        DeptShort2 = c.String(maxLength: 100),
                        DeptShort3 = c.String(maxLength: 100),
                        DeptShort4 = c.String(maxLength: 100),
                        DeptShort5 = c.String(maxLength: 100),
                        DeptShort6 = c.String(maxLength: 100),
                        DeptShort7 = c.String(maxLength: 100),
                        DeptFull = c.String(maxLength: 500),
                        DeptFull1 = c.String(maxLength: 500),
                        DeptFull2 = c.String(maxLength: 500),
                        DeptFull3 = c.String(maxLength: 500),
                        DeptFull4 = c.String(maxLength: 500),
                        DeptFull5 = c.String(maxLength: 500),
                        DeptFull6 = c.String(maxLength: 500),
                        DeptFull7 = c.String(maxLength: 500),
                        DeptFullEng = c.String(maxLength: 500),
                        DeptFullEng1 = c.String(maxLength: 500),
                        DeptFullEng2 = c.String(maxLength: 500),
                        DeptFullEng3 = c.String(maxLength: 500),
                        DeptFullEng4 = c.String(maxLength: 500),
                        DeptFullEng5 = c.String(maxLength: 500),
                        DeptFullEng6 = c.String(maxLength: 500),
                        DeptFullEng7 = c.String(maxLength: 500),
                        DeptStatus = c.String(maxLength: 100),
                        DeptClass = c.String(maxLength: 100),
                        DeptArea = c.String(maxLength: 100),
                        DeptTel = c.String(maxLength: 100),
                        DeptEffectDate = c.DateTime(),
                        DeptOrderNo = c.String(maxLength: 100),
                        DeptEffectOff = c.DateTime(),
                        DeptAt = c.String(maxLength: 100),
                        DeptMoiCode = c.String(maxLength: 100),
                        CostCenterCode = c.String(maxLength: 10),
                        CostCenterName = c.String(maxLength: 200),
                        DeptOrder = c.String(maxLength: 2),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.DeptSap, unique: true)
                .Index(t => t.DeptUpper)
                .Index(t => t.CostCenterCode);
            
            CreateTable(
                "dbo.LevelInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LevelCode = c.String(maxLength: 2),
                        LevelDesc = c.String(maxLength: 100),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.LevelCode, unique: true);
            
            CreateTable(
                "dbo.PeaInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PeaCode = c.String(maxLength: 6),
                        PeaShortName = c.String(maxLength: 100),
                        PeaName = c.String(maxLength: 200),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.PeaCode, unique: true);
            
            CreateTable(
                "dbo.PositionInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PositionCode = c.String(maxLength: 4),
                        PositionDescShort = c.String(maxLength: 100),
                        PostionDesc = c.String(maxLength: 200),
                        CreatedBy = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.PositionCode, unique: true);
            
            AddColumn("dbo.Employee", "PositionCode", c => c.String(maxLength: 4));
            AddColumn("dbo.Employee", "LevelCode", c => c.String(maxLength: 2));
            AddColumn("dbo.Employee", "Email", c => c.String(maxLength: 200));
            AddColumn("dbo.Employee", "DepartmentSap", c => c.Int());
            AddColumn("dbo.Employee", "StaffDate", c => c.String(maxLength: 10));
            AddColumn("dbo.Employee", "EntryDate", c => c.String(maxLength: 10));
            AddColumn("dbo.Employee", "RetiredDate", c => c.String(maxLength: 10));
            AddColumn("dbo.Employee", "BaCode", c => c.String(maxLength: 4));
            AddColumn("dbo.Employee", "PeaCode", c => c.String(maxLength: 6));
            AddColumn("dbo.Employee", "StatusCode", c => c.String(maxLength: 1));
            AddColumn("dbo.Employee", "StatusName", c => c.String(maxLength: 100));
            AddColumn("dbo.Employee", "Group", c => c.Int(nullable: false));
            AddColumn("dbo.User", "PasswordHash", c => c.String(maxLength: 500));
            DropColumn("dbo.Employee", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employee", "Password", c => c.String(maxLength: 32));
            DropIndex("dbo.PositionInfo", new[] { "PositionCode" });
            DropIndex("dbo.PeaInfo", new[] { "PeaCode" });
            DropIndex("dbo.LevelInfo", new[] { "LevelCode" });
            DropIndex("dbo.DepartmentInfo", new[] { "CostCenterCode" });
            DropIndex("dbo.DepartmentInfo", new[] { "DeptUpper" });
            DropIndex("dbo.DepartmentInfo", new[] { "DeptSap" });
            DropIndex("dbo.BusinessAreaInfo", new[] { "BaCode" });
            DropColumn("dbo.User", "PasswordHash");
            DropColumn("dbo.Employee", "Group");
            DropColumn("dbo.Employee", "StatusName");
            DropColumn("dbo.Employee", "StatusCode");
            DropColumn("dbo.Employee", "PeaCode");
            DropColumn("dbo.Employee", "BaCode");
            DropColumn("dbo.Employee", "RetiredDate");
            DropColumn("dbo.Employee", "EntryDate");
            DropColumn("dbo.Employee", "StaffDate");
            DropColumn("dbo.Employee", "DepartmentSap");
            DropColumn("dbo.Employee", "Email");
            DropColumn("dbo.Employee", "LevelCode");
            DropColumn("dbo.Employee", "PositionCode");
            DropTable("dbo.PositionInfo");
            DropTable("dbo.PeaInfo");
            DropTable("dbo.LevelInfo");
            DropTable("dbo.DepartmentInfo");
            DropTable("dbo.BusinessAreaInfo");
        }
    }
}
