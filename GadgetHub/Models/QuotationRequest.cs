using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadgetHub.Models
{
    public class QuotationRequest
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int DistributorId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public OrderItem OrderItem { get; set; }
        public Distributor Distributor { get; set; }
    }
}