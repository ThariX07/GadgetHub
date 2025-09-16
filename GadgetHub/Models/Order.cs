using Newtonsoft.Json;
using System;

namespace GadgetHub.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? CustomerId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        [JsonIgnore] // Prevent circular reference
        public virtual Product Product { get; set; }
    }
}