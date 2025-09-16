namespace GadgetHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderAndQuotationModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "RequestDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Quotations", "OrderId", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "OrderDate");
            DropColumn("dbo.Quotations", "OrderItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Quotations", "OrderItemId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "OrderDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Quotations", "OrderId");
            DropColumn("dbo.Orders", "RequestDate");
        }
    }
}
