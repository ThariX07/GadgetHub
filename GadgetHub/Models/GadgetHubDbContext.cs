using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GadgetHub.Models
{
    public class GadgetHubDbContext : DbContext
    {
        public GadgetHubDbContext() : base("name=GadgetHubDb")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<QuotationRequest> QuotationRequests { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<DistributorOrder> DistributorOrders { get; set; }
        public DbSet<OrderConfirmation> OrderConfirmations { get; set; }
    }
}