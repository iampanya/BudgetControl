namespace BudgetControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToYearColumn : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Payment", "Year");
            CreateIndex("dbo.PaymentCounter", "Year");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PaymentCounter", new[] { "Year" });
            DropIndex("dbo.Payment", new[] { "Year" });
        }
    }
}
