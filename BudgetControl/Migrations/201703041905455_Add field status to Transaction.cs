namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddfieldstatustoTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetTransaction", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetTransaction", "Status");
        }
    }
}
