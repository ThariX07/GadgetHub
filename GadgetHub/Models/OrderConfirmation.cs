using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadgetHub.Models
{
    public class OrderConfirmation
    {
        public int Id { get; set; }
        public int DistributorOrderId { get; set; }
        public string DistributorOrderRef { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DistributorOrder DistributorOrder { get; set; }
    }
}