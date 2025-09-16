using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadgetHub.Models
{
    public class DistributorOrder
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int DistributorId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderItem OrderItem { get; set; }
        public Distributor Distributor { get; set; }
    }
}