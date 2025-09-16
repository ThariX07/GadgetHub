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
    public class OrderConfirmationsController : ApiController
    {
        private GadgetHubDbContext db = new GadgetHubDbContext();

        // GET: api/OrderConfirmations
        public IQueryable<OrderConfirmation> GetOrderConfirmations()
        {
            return db.OrderConfirmations;
        }

        // GET: api/OrderConfirmations/5
        [ResponseType(typeof(OrderConfirmation))]
        public async Task<IHttpActionResult> GetOrderConfirmation(int id)
        {
            OrderConfirmation orderConfirmation = await db.OrderConfirmations.FindAsync(id);
            if (orderConfirmation == null)
            {
                return NotFound();
            }

            return Ok(orderConfirmation);
        }

        // PUT: api/OrderConfirmations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrderConfirmation(int id, OrderConfirmation orderConfirmation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderConfirmation.Id)
            {
                return BadRequest();
            }

            db.Entry(orderConfirmation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderConfirmationExists(id))
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

        // POST: api/OrderConfirmations
        [ResponseType(typeof(OrderConfirmation))]
        public async Task<IHttpActionResult> PostOrderConfirmation(OrderConfirmation orderConfirmation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderConfirmations.Add(orderConfirmation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orderConfirmation.Id }, orderConfirmation);
        }

        // DELETE: api/OrderConfirmations/5
        [ResponseType(typeof(OrderConfirmation))]
        public async Task<IHttpActionResult> DeleteOrderConfirmation(int id)
        {
            OrderConfirmation orderConfirmation = await db.OrderConfirmations.FindAsync(id);
            if (orderConfirmation == null)
            {
                return NotFound();
            }

            db.OrderConfirmations.Remove(orderConfirmation);
            await db.SaveChangesAsync();

            return Ok(orderConfirmation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderConfirmationExists(int id)
        {
            return db.OrderConfirmations.Count(e => e.Id == id) > 0;
        }
    }
}