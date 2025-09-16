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
    public class DistributorOrdersController : ApiController
    {
        private GadgetHubDbContext db = new GadgetHubDbContext();

        // GET: api/DistributorOrders
        public IQueryable<DistributorOrder> GetDistributorOrders()
        {
            return db.DistributorOrders;
        }

        // GET: api/DistributorOrders/5
        [ResponseType(typeof(DistributorOrder))]
        public async Task<IHttpActionResult> GetDistributorOrder(int id)
        {
            DistributorOrder distributorOrder = await db.DistributorOrders.FindAsync(id);
            if (distributorOrder == null)
            {
                return NotFound();
            }

            return Ok(distributorOrder);
        }

        // PUT: api/DistributorOrders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDistributorOrder(int id, DistributorOrder distributorOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != distributorOrder.Id)
            {
                return BadRequest();
            }

            db.Entry(distributorOrder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistributorOrderExists(id))
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

        // POST: api/DistributorOrders
        [ResponseType(typeof(DistributorOrder))]
        public async Task<IHttpActionResult> PostDistributorOrder(DistributorOrder distributorOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DistributorOrders.Add(distributorOrder);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = distributorOrder.Id }, distributorOrder);
        }

        // DELETE: api/DistributorOrders/5
        [ResponseType(typeof(DistributorOrder))]
        public async Task<IHttpActionResult> DeleteDistributorOrder(int id)
        {
            DistributorOrder distributorOrder = await db.DistributorOrders.FindAsync(id);
            if (distributorOrder == null)
            {
                return NotFound();
            }

            db.DistributorOrders.Remove(distributorOrder);
            await db.SaveChangesAsync();

            return Ok(distributorOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DistributorOrderExists(int id)
        {
            return db.DistributorOrders.Count(e => e.Id == id) > 0;
        }
    }
}