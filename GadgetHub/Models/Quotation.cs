using System;

namespace GadgetHub.Models
{
    public class Quotation
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public decimal? QuoteAmount { get; set; }
        public int? DistributorId { get; set; }
        public DateTime? ResponseDate { get; set; }
        public virtual Order Order { get; set; } // Navigation property
    }
}