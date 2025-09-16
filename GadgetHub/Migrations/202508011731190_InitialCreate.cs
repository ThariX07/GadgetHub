namespace GadgetHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PasswordHash = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DistributorOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderItemId = c.Int(nullable: false),
                        DistributorId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Distributors", t => t.DistributorId, cascadeDelete: true)
                .ForeignKey("dbo.OrderItems", t => t.OrderItemId, cascadeDelete: true)
                .Index(t => t.OrderItemId)
                .Index(t => t.DistributorId);
            
            CreateTable(
                "dbo.Distributors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        OrderDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderConfirmations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DistributorOrderId = c.Int(nullable: false),
                        DistributorOrderRef = c.String(),
                        EstimatedDeliveryDate = c.DateTime(nullable: false),
                        ConfirmationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributorOrders", t => t.DistributorOrderId, cascadeDelete: true)
                .Index(t => t.DistributorOrderId);
            
            CreateTable(
                "dbo.QuotationRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderItemId = c.Int(nullable: false),
                        DistributorId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Distributors", t => t.DistributorId, cascadeDelete: true)
                .ForeignKey("dbo.OrderItems", t => t.OrderItemId, cascadeDelete: true)
                .Index(t => t.OrderItemId)
                .Index(t => t.DistributorId);
            
            CreateTable(
                "dbo.Quotations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderItemId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        QuoteAmount = c.Decimal(precision: 18, scale: 2),
                        DistributorId = c.Int(),
                        ResponseDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuotationRequests", "OrderItemId", "dbo.OrderItems");
            DropForeignKey("dbo.QuotationRequests", "DistributorId", "dbo.Distributors");
            DropForeignKey("dbo.OrderConfirmations", "DistributorOrderId", "dbo.DistributorOrders");
            DropForeignKey("dbo.DistributorOrders", "OrderItemId", "dbo.OrderItems");
            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.DistributorOrders", "DistributorId", "dbo.Distributors");
            DropIndex("dbo.QuotationRequests", new[] { "DistributorId" });
            DropIndex("dbo.QuotationRequests", new[] { "OrderItemId" });
            DropIndex("dbo.OrderConfirmations", new[] { "DistributorOrderId" });
            DropIndex("dbo.OrderItems", new[] { "ProductId" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropIndex("dbo.DistributorOrders", new[] { "DistributorId" });
            DropIndex("dbo.DistributorOrders", new[] { "OrderItemId" });
            DropTable("dbo.Quotations");
            DropTable("dbo.QuotationRequests");
            DropTable("dbo.OrderConfirmations");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Distributors");
            DropTable("dbo.DistributorOrders");
            DropTable("dbo.Customers");
        }
    }
}
