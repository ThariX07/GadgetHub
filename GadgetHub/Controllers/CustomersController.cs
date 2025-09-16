using GadgetHub.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace GadgetHub.Controllers
{
    public class CustomersController : ApiController
    {
        private GadgetHubDbContext db = new GadgetHubDbContext();

        // ✅ Login
        [HttpPost]
        [Route("api/Customers/Login")]
        public IHttpActionResult Login([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var customer = db.Customers.FirstOrDefault(c => c.Email == login.Email && c.PasswordHash == login.Password); // Simple match, use hash in production
            if (customer == null)
            {
                return Unauthorized();
            }

            return Ok(new { CustomerId = customer.Id });
        }

        [HttpPost]
        [Route("api/Customers/Register")]
        public IHttpActionResult Register([FromBody] Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.Name) ||
                string.IsNullOrWhiteSpace(customer.Email) || string.IsNullOrWhiteSpace(customer.PasswordHash))
            {
                return BadRequest("All fields are required.");
            }

            // Optional: basic unique-email check
            if (db.Customers.Any(c => c.Email == customer.Email))
            {
                return Content(HttpStatusCode.Conflict, new { Message = "Email already in use." });
            }

            db.Customers.Add(customer);
            db.SaveChanges();
            return Ok(new { CustomerId = customer.Id, Message = "Registered successfully." });
        }


        // ✅ Get Customer by ID
        [HttpGet]
        [Route("api/Customers/{id:int}")]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // ✅ Update Customer
        [HttpPut]
        [Route("api/Customers/{id:int}")]
        public IHttpActionResult UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            if (updatedCustomer == null || id != updatedCustomer.Id)
            {
                return BadRequest("Invalid customer data.");
            }

            var existingCustomer = db.Customers.Find(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.PasswordHash = updatedCustomer.PasswordHash;

            db.Entry(existingCustomer).State = EntityState.Modified;
            db.SaveChanges();

            return Ok("Customer updated successfully.");
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
}
