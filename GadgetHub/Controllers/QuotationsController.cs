using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GadgetHub.Models;

namespace GadgetHub.Controllers
{
    public class QuotationsController : ApiController
    {
        private GadgetHubDbContext db = new GadgetHubDbContext();

        [HttpPost]
        [Route("api/Quotations/Respond")]
        public async Task<IHttpActionResult> RespondToQuotation([FromBody] QuotationResponseModel quote)
        {
            if (quote == null || quote.QuotationId <= 0 || quote.QuoteAmount <= 0 || quote.DistributorId == null)
            {
                return BadRequest("Invalid quotation response data.");
            }

            var quotation = await db.Quotations.FindAsync(quote.QuotationId);
            if (quotation == null || quotation.Status != "Pending")
            {
                return NotFound();
            }

            quotation.QuoteAmount = quote.QuoteAmount;
            quotation.DistributorId = quote.DistributorId;
            quotation.Status = "Quoted";
            quotation.ResponseDate = DateTime.Now;
            await db.SaveChangesAsync();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class QuotationResponseModel
    {
        public int QuotationId { get; set; }
        public decimal QuoteAmount { get; set; }
        public int? DistributorId { get; set; }
    }
}