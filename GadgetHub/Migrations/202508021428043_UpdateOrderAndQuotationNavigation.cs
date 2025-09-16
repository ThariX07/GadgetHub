namespace GadgetHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderAndQuotationNavigation : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Orders", "ProductId");
            CreateIndex("dbo.Quotations", "OrderId");
            AddForeignKey("dbo.Orders", "ProductId", "dbo.Products", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Quotations", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotations", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropIndex("dbo.Quotations", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "ProductId" });
        }
    }
}
