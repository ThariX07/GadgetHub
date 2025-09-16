using GadgetHub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace GadgetHub.Controllers
{
    public class ProductsController : ApiController
    {
        private GadgetHubDbContext db = new GadgetHubDbContext();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products/UploadImage
        [HttpPost]
        [Route("api/Products/UploadImage")]
        public async Task<IHttpActionResult> UploadImage()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Expected multipart/form-data");

            var root = HttpContext.Current.Server.MapPath("~/images");
            Directory.CreateDirectory(root);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.FirstOrDefault();
            if (file == null)
                return BadRequest("No file uploaded.");

            var originalName = file.Headers.ContentDisposition.FileName?.Trim('\"') ?? "upload";
            var ext = Path.GetExtension(originalName);
            var allowed = new[] { ".png", ".jpg", ".jpeg", ".webp", ".gif" };
            if (!allowed.Contains(ext, StringComparer.OrdinalIgnoreCase))
                return BadRequest("Unsupported file type.");

            var bytes = await file.ReadAsByteArrayAsync();

            // (Optional) size guard: 5MB
            if (bytes.Length > 5 * 1024 * 1024)
                return BadRequest("File too large (max 5MB).");

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var savePath = Path.Combine(root, fileName);
            File.WriteAllBytes(savePath, bytes);

            // absolute URL back to the API site (https://localhost:44376)
            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var url = $"{baseUrl}/images/{fileName}";

            // Return both absolute URL and app-relative path (if you ever want it)
            return Ok(new { url, relative = $"/images/{fileName}" });
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}