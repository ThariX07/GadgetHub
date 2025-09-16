using GadgetHub.Models;
using GadgetHubClient.Services;
using System;
using System.Linq;

namespace GadgetHubClient
{
    public partial class products : System.Web.UI.Page
    {
        private ProductServices productService = new ProductServices();

        private string currentOrderId
        {
            get { return ViewState["currentOrderId"] as string ?? string.Empty; }
            set { ViewState["currentOrderId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CustomerId"] != null)
                {
                    LoadProducts();
                    LoadLatestOrderAndQuote();
                    LoadCustomerProfile();
                }
                else
                {
                    Response.Redirect("CustomerLogin.aspx", false);
                }
            }
        }

        private void LoadProducts()
        {
            var products = productService.GetAllProducts();
            gvProducts.DataSource = products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                ImageUrl = p.ImageUrl ?? ""
            }).ToList();
            gvProducts.DataBind();
        }

        private void LoadCustomerProfile()
        {
            try
            {
                if (Session["CustomerId"] != null)
                {
                    int customerId = Convert.ToInt32(Session["CustomerId"]);
                    var customer = productService.GetCustomerById(customerId);

                    if (customer != null)
                    {
                        txtProfileName.Text = customer.Name;
                        txtProfileEmail.Text = customer.Email;
                        txtProfilePassword.Text = customer.PasswordHash;
                    }
                }
            }
            catch (Exception ex)
            {
                lblProfileMessage.Text = "Error loading profile: " + ex.Message;
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["CustomerId"] != null)
                {
                    int customerId = Convert.ToInt32(Session["CustomerId"]);

                    Customer updatedCustomer = new Customer
                    {
                        Id = customerId,
                        Name = txtProfileName.Text.Trim(),
                        Email = txtProfileEmail.Text.Trim(),
                        PasswordHash = txtProfilePassword.Text.Trim()
                    };

                    bool result = productService.UpdateCustomer(updatedCustomer);

                    if (result)
                    {
                        lblProfileMessage.Text = "Profile updated successfully!";
                        lblProfileMessage.CssClass = "message message-success";
                    }
                    else
                    {
                        lblProfileMessage.Text = "Failed to update profile.";
                        lblProfileMessage.CssClass = "message message-error";
                    }
                }
            }
            catch (Exception ex)
            {
                lblProfileMessage.Text = "Error: " + ex.Message;
                lblProfileMessage.CssClass = "message message-error";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Default.aspx", false); // Redirect to home page
        }

        protected void gvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["CustomerId"] != null)
            {
                lblProductId.Visible = true;
                lblSelectedProductId.Visible = true;
                lblSelectedProductId.Text = gvProducts.SelectedRow.Cells[1].Text;
                btnSubmitRequest.Enabled = true;
            }
            else
            {
                Response.Redirect("CustomerLogin.aspx", false);
            }
        }

        protected void btnSubmitRequest_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = int.Parse(lblSelectedProductId.Text.Split('-')[0].Trim());
                int quantity = int.Parse(txtQuantity.Text);
                int customerId = Convert.ToInt32(Session["CustomerId"]);

                var request = new { ProductId = productId, Quantity = quantity, CustomerId = customerId };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44376/");
                    var response = client.PostAsync("api/Orders/Request", content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        lblRequestMessage.Text = "Failed to send request.";
                        lblRequestMessage.CssClass = "message message-error";
                        return;
                    }

                    var result = response.Content.ReadAsStringAsync().Result;
                    dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                    currentOrderId = responseData.OrderId?.ToString();

                    lblRequestMessage.Text = "Request submitted! Awaiting quote.";
                    lblRequestMessage.CssClass = "message message-success";
                    btnPlaceOrder.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblRequestMessage.Text = "Error: " + ex.Message;
                lblRequestMessage.CssClass = "message message-error";
            }
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(currentOrderId))
                {
                    lblOrderMessage.Text = "No order available.";
                    return;
                }

                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44376/");
                    var response = client.PostAsync($"api/Orders/Confirm/{currentOrderId}", null).Result;
                    response.EnsureSuccessStatusCode();
                    var result = response.Content.ReadAsStringAsync().Result;
                    dynamic order = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);

                    lblOrderMessage.Text = $"Order placed! ID: {order.OrderId}, Product: {order.ProductName}";
                    lblOrderMessage.CssClass = "message message-success";
                    btnPlaceOrder.Enabled = false;
                    lblCheapestQuote.Text = "Cheapest Quote: Order confirmed";
                    currentOrderId = null;
                }
            }
            catch (Exception ex)
            {
                lblOrderMessage.Text = "Error placing order: " + ex.Message;
                lblOrderMessage.CssClass = "message message-error";
            }
        }

        private void LoadLatestOrderAndQuote()
        {
            try
            {
                if (Session["CustomerId"] == null)
                    return;

                int customerId = (int)Session["CustomerId"];
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44376/");
                    var response = client.GetAsync($"api/Orders/LatestPendingOrder/{customerId}").Result;
                    if (!response.IsSuccessStatusCode) return;

                    var result = response.Content.ReadAsStringAsync().Result;
                    dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                    currentOrderId = responseData.OrderId?.ToString();

                    if (!string.IsNullOrEmpty(currentOrderId))
                    {
                        var quoteResponse = client.GetAsync($"api/Orders/CheapestQuote/{currentOrderId}").Result;
                        if (!quoteResponse.IsSuccessStatusCode) return;

                        var quoteResult = quoteResponse.Content.ReadAsStringAsync().Result;
                        var quote = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(quoteResult);
                        if (quote?.Price != null && quote?.ProductName != null)
                        {
                            lblCheapestQuote.Text = $"Cheapest Quote: $${quote.Price} for {quote.ProductName}";
                            btnPlaceOrder.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblCheapestQuote.Text = "Cheapest Quote: Error loading.";
                btnPlaceOrder.Enabled = false;
            }
        }
    }
}
