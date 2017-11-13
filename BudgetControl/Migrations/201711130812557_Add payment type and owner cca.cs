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
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payment", "Type");
            DropColumn("dbo.Payment", "OwnerCostCenterID");
        }
    }
}
