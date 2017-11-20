namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addpaymenttypeandownercca : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payment", "OwnerCostCenterID", c => c.String());
            AddColumn("dbo.Payment", "Type", c => c.Int(nullable: false));

            Sql("UPDATE Payment SET [Type] = 1 WHERE CostCenterID NOT LIKE 'Z%'");
            Sql("UPDATE Payment SET [Type] = 2 WHERE CostCenterID LIKE 'Z%'");
            Sql("UPDATE Payment SET [OwnerCostCenterID] = [CostCenterID] WHERE CostCenterID NOT LIKE 'Z%'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payment", "Type");
            DropColumn("dbo.Payment", "OwnerCostCenterID");
        }
    }
}
