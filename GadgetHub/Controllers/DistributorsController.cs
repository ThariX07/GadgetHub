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
    public class DistributorsController : ApiController
    {
        private GadgetHubDbContext db = new GadgetHubDbContext();

        // GET: api/Distributors
        public IQueryable<Distributor> GetDistributors()
        {
            return db.Distributors;
        }

        // GET: api/Distributors/5
        [ResponseType(typeof(Distributor))]
        public async Task<IHttpActionResult> GetDistributor(int id)
        {
            Distributor distributor = await db.Distributors.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }
            return Ok(distributor);
        }

        // PUT: api/Distributors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDistributor(int id, Distributor distributor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != distributor.Id)
            {
                return BadRequest();
            }
            db.Entry(distributor).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistributorExists(id))
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

        // POST: api/Distributors
        [ResponseType(typeof(Distributor))]
        public async Task<IHttpActionResult> PostDistributor(Distributor distributor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Distributors.Add(distributor);
            await db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = distributor.Id }, distributor);
        }

        // DELETE: api/Distributors/5
        [ResponseType(typeof(Distributor))]
        public async Task<IHttpActionResult> DeleteDistributor(int id)
        {
            Distributor distributor = await db.Distributors.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }
            db.Distributors.Remove(distributor);
            await db.SaveChangesAsync();
            return Ok(distributor);
        }

        // POST: api/Distributors/Login
        [HttpPost]
        [Route("api/Distributors/Login")]
        public IHttpActionResult Login([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var distributor = db.Distributors.FirstOrDefault(d => d.Email == login.Email && d.PasswordHash == login.Password); // Use proper hashing in production
            if (distributor == null)
            {
                return Unauthorized();
            }

            return Ok(new { DistributorId = distributor.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DistributorExists(int id)
        {
            return db.Distributors.Count(e => e.Id == id) > 0;
        }
    }
}