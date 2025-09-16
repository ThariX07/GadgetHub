using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GadgetHub.Models;

namespace GadgetHub.Controllers
{
    public class OrdersController : ApiController
    {
        private GadgetHubDbContext db = new GadgetHubDbContext();

        [HttpPost]
        [Route("api/Orders/Request")]
        public async Task<IHttpActionResult> CreateRequest([FromBody] OrderRequestModel request)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("CreateRequest: New call received - ProductId: " + (request?.ProductId ?? 0) + ", Quantity: " + (request?.Quantity ?? 0) + ", CustomerId: " + (request?.CustomerId ?? 0));
                if (request == null || request.ProductId <= 0 || request.Quantity <= 0 || request.CustomerId == null)
                {
                    System.Diagnostics.Debug.WriteLine("CreateRequest: Invalid request data - ProductId: " + (request?.ProductId ?? 0) + ", Quantity: " + (request?.Quantity ?? 0) + ", CustomerId: " + (request?.CustomerId ?? 0));
                    return BadRequest("Invalid request data.");
                }

                var product = await db.Products.FindAsync(request.ProductId);
                if (product == null)
                {
                    System.Diagnostics.Debug.WriteLine("CreateRequest: Product not found for ProductId: " + request.ProductId);
                    return Content(HttpStatusCode.NotFound, new { Message = "Product not found." });
                }

                var order = new Order
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    CustomerId = request.CustomerId,
                    RequestDate = DateTime.Now,
                    Status = "Pending"
                };
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("CreateRequest: Order created - OrderId: " + order.Id);
                return Ok(new { OrderId = order.Id });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CreateRequest: Exception - " + ex.Message + " | StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet]
        [Route("api/Orders/PendingRequests")]
        public IQueryable<Order> GetPendingRequests()
        {
            try
            {
                var pendingOrders = db.Orders.Where(o => o.Status != null && o.Status == "Pending");
                if (pendingOrders.Any())
                {
                    System.Diagnostics.Debug.WriteLine("Pending orders found: " + pendingOrders.Count());
                    return pendingOrders;
                }
                System.Diagnostics.Debug.WriteLine("No pending orders found.");
                return Enumerable.Empty<Order>().AsQueryable();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in GetPendingRequests: " + ex.Message + " | StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet]
        [Route("api/Orders/PendingRequestsForDistributor/{distributorId}")]
        public IQueryable<Order> GetPendingRequestsForDistributor(int distributorId)
        {
            try
            {
                var ordersWithQuotes = db.Quotations.Where(q => q.DistributorId == distributorId && q.Status == "Quoted").Select(q => q.OrderId);
                var pendingOrders = db.Orders.Where(o => o.Status != null && o.Status == "Pending" && !ordersWithQuotes.Contains(o.Id));
                if (pendingOrders.Any())
                {
                    System.Diagnostics.Debug.WriteLine("Pending orders for Distributor " + distributorId + ": " + pendingOrders.Count());
                    return pendingOrders;
                }
                System.Diagnostics.Debug.WriteLine("No pending orders found for Distributor " + distributorId);
                return Enumerable.Empty<Order>().AsQueryable();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in GetPendingRequestsForDistributor: " + ex.Message + " | StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        [Route("api/Orders/SubmitQuote")]
        public async Task<IHttpActionResult> SubmitQuote([FromBody] QuoteModel quote)
        {
            if (quote == null || quote.OrderId <= 0 || quote.Price <= 0 || quote.DistributorId == null)
            {
                return BadRequest("Invalid quote data.");
            }

            var order = await db.Orders.FindAsync(quote.OrderId);
            if (order == null || order.Status != "Pending")
            {
                return NotFound();
            }

            var product = await db.Products.FindAsync(order.ProductId);
            if (product == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = "Product not found." });
            }

            var quotation = new Quotation
            {
                OrderId = quote.OrderId,
                QuoteAmount = quote.Price,
                DistributorId = quote.DistributorId,
                RequestDate = order.RequestDate,
                Status = "Quoted",
                ResponseDate = DateTime.Now
            };
            db.Quotations.Add(quotation);
            await db.SaveChangesAsync();
            System.Diagnostics.Debug.WriteLine("SubmitQuote: Quote submitted - OrderId: " + quote.OrderId + ", Price: " + quote.Price + ", ProductName: " + product.Name);
            return Ok(new { OrderId = quote.OrderId, Price = quote.Price, ProductName = product.Name });
        }

        [HttpGet]
        [Route("api/Orders/LatestQuote/{orderId}")]
        public IHttpActionResult GetLatestQuote(int orderId)
        {
            try
            {
                var latestQuote = (from q in db.Quotations
                                   join o in db.Orders on q.OrderId equals o.Id
                                   join p in db.Products on o.ProductId equals p.Id
                                   where q.OrderId == orderId && q.Status == "Quoted"
                                   orderby q.ResponseDate descending
                                   select new
                                   {
                                       Price = q.QuoteAmount,
                                       ProductName = p.Name
                                   }).FirstOrDefault();
                var priceStr = latestQuote?.Price?.ToString() ?? "null";
                var productNameStr = latestQuote?.ProductName ?? "null";
                System.Diagnostics.Debug.WriteLine("GetLatestQuote: OrderId " + orderId + ", Result - Price: " + priceStr + ", ProductName: " + productNameStr);
                return Ok(latestQuote ?? new { Price = (decimal?)null, ProductName = (string)null });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetLatestQuote: Error - " + ex.Message + " | StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet]
        [Route("api/Orders/CheapestQuote/{orderId}")]
        public IHttpActionResult GetCheapestQuote(int orderId)
        {
            try
            {
                var cheapestQuote = (from q in db.Quotations
                                     join o in db.Orders on q.OrderId equals o.Id
                                     join p in db.Products on o.ProductId equals p.Id
                                     where q.OrderId == orderId && q.Status == "Quoted"
                                     orderby q.QuoteAmount
                                     select new
                                     {
                                         Price = q.QuoteAmount,
                                         ProductName = p.Name
                                     }).FirstOrDefault();
                var priceStr = cheapestQuote?.Price?.ToString() ?? "null";
                var productNameStr = cheapestQuote?.ProductName ?? "null";
                System.Diagnostics.Debug.WriteLine("GetCheapestQuote: OrderId " + orderId + ", Result - Price: " + priceStr + ", ProductName: " + productNameStr);
                return Ok(cheapestQuote ?? new { Price = (decimal?)null, ProductName = (string)null });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetCheapestQuote: Error - " + ex.Message + " | StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet]
        [Route("api/Orders/LatestPendingOrder/{customerId}")]
        public IHttpActionResult GetLatestPendingOrder(int customerId)
        {
            try
            {
                var latestOrder = db.Orders
                    .Where(o => o.CustomerId == customerId && o.Status == "Pending")
                    .OrderByDescending(o => o.RequestDate)
                    .FirstOrDefault();
                if (latestOrder != null)
                {
                    System.Diagnostics.Debug.WriteLine("GetLatestPendingOrder: Found order - OrderId: " + latestOrder.Id);
                    return Ok(new { OrderId = latestOrder.Id });
                }
                System.Diagnostics.Debug.WriteLine("GetLatestPendingOrder: No pending order found for CustomerId: " + customerId);
                return Ok(new { OrderId = (int?)null });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetLatestPendingOrder: Error - " + ex.Message + " | StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        [Route("api/Orders/Confirm/{orderId}")]
        public async Task<IHttpActionResult> ConfirmOrder(int orderId)
        {
            var order = await db.Orders.FindAsync(orderId);
            if (order == null || order.Status != null && order.Status != "Pending")
            {
                return NotFound();
            }
            var latestQuote = db.Quotations
                .Where(q => q.OrderId == orderId && q.Status == "Quoted")
                .OrderByDescending(q => q.ResponseDate)
                .FirstOrDefault();
            if (latestQuote != null)
            {
                order.Status = "Confirmed";
                latestQuote.Status = "Accepted";
                await db.SaveChangesAsync();
                var product = await db.Products.FindAsync(order.ProductId);
                System.Diagnostics.Debug.WriteLine("ConfirmOrder: Order confirmed - OrderId: " + orderId + ", ProductName: " + product.Name);
                return Ok(new { OrderId = orderId, ProductId = order.ProductId, Quantity = order.Quantity, ProductName = product.Name });
            }
            return Content(HttpStatusCode.NotFound, new { Message = "No valid quote found." });
        }

        [HttpGet]
        [Route("api/Orders/Confirmed/{distributorId}")]
        public IQueryable<dynamic> GetConfirmedOrders(int distributorId)
        {
            return from o in db.Orders
                   join q in db.Quotations on o.Id equals q.OrderId
                   where q.DistributorId == distributorId && o.Status == "Confirmed" && q.Status == "Accepted"
                   select new
                   {
                       OrderId = o.Id,
                       ProductId = o.ProductId,
                       Quantity = o.Quantity,
                       ProductName = o.Product.Name,
                       Price = q.QuoteAmount
                   };
        }

        [HttpDelete]
        [Route("api/Orders/Delete/{orderId}")]
        public async Task<IHttpActionResult> DeleteOrder(int orderId)
        {
            var order = await db.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Also remove related quotation(s)
            var relatedQuotes = db.Quotations.Where(q => q.OrderId == orderId).ToList();
            db.Quotations.RemoveRange(relatedQuotes);

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            System.Diagnostics.Debug.WriteLine($"[DeleteOrder] Deleted Order ID: {orderId}");
            return Ok(new { Message = "Order deleted successfully." });
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

    public class OrderRequestModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? CustomerId { get; set; }
    }

    public class QuoteModel
    {
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int? DistributorId { get; set; }
    }
}