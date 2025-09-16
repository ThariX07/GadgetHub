using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GadgetHub.Models;

namespace GadgetHub.Controllers
{
    public class QuotationRequestsController : ApiController
    {
        private GadgetHubDbContext db = new GadgetHubDbContext();

        // GET: api/QuotationRequests
        public IQueryable<QuotationRequest> GetQuotationRequests()
        {
            return db.QuotationRequests;
        }

        // GET: api/QuotationRequests/5
        [ResponseType(typeof(QuotationRequest))]
        public async Task<IHttpActionResult> GetQuotationRequest(int id)
        {
            QuotationRequest quotationRequest = await db.QuotationRequests.FindAsync(id);
            if (quotationRequest == null)
            {
                return NotFound();
            }

            return Ok(quotationRequest);
        }

        // PUT: api/QuotationRequests/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQuotationRequest(int id, QuotationRequest quotationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quotationRequest.Id)
            {
                return BadRequest();
            }

            db.Entry(quotationRequest).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuotationRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/QuotationRequests
        [ResponseType(typeof(QuotationRequest))]
        public async Task<IHttpActionResult> PostQuotationRequest(QuotationRequest quotationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.QuotationRequests.Add(quotationRequest);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = quotationRequest.Id }, quotationRequest);
        }

        // DELETE: api/QuotationRequests/5
        [ResponseType(typeof(QuotationRequest))]
        public async Task<IHttpActionResult> DeleteQuotationRequest(int id)
        {
            QuotationRequest quotationRequest = await db.QuotationRequests.FindAsync(id);
            if (quotationRequest == null)
            {
                return NotFound();
            }

            db.QuotationRequests.Remove(quotationRequest);
            await db.SaveChangesAsync();

            return Ok(quotationRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuotationRequestExists(int id)
        {
            return db.QuotationRequests.Count(e => e.Id == id) > 0;
        }
    }
}