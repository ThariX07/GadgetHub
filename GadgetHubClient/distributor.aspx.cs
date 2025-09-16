using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.UI.WebControls;
using GadgetHub.Models;

namespace GadgetHubClient
{
    public partial class distributor : System.Web.UI.Page
    {
        protected GridView gvPendingOrders;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["DistributorId"] != null)
            {
                LoadPendingRequests();
                LoadConfirmedOrders();
            }
        }

        protected void gvConfirmedOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CompleteOrder")
            {
                try
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = gvConfirmedOrders.Rows[rowIndex];
                    int orderId = Convert.ToInt32(row.Cells[0].Text); // Order ID is in first column

                    using (var client = new HttpClient { BaseAddress = new Uri("https://localhost:44376/") })
                    {
                        var response = client.DeleteAsync($"api/Orders/Delete/{orderId}").Result;

                        if (response.IsSuccessStatusCode)
                        {
                            lblConfirmedOrdersMessage.Text = "Order completed and removed successfully.";
                            lblConfirmedOrdersMessage.CssClass = "message message-success";
                            btnLoadConfirmedOrders_Click(null, null); // Refresh
                        }
                        else
                        {
                            lblConfirmedOrdersMessage.Text = "Failed to complete order.";
                            lblConfirmedOrdersMessage.CssClass = "message message-error";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblConfirmedOrdersMessage.Text = "Error: " + ex.Message;
                    lblConfirmedOrdersMessage.CssClass = "message message-error";
                }
            }
        }



        protected void btnLoadConfirmedOrders_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DistributorId"] != null)
                {
                    int distributorId = Convert.ToInt32(Session["DistributorId"]);
                    using (var client = new HttpClient { BaseAddress = new Uri("https://localhost:44376/") })
                    {
                        var response = client.GetAsync($"api/Orders/Confirmed/{distributorId}").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            var confirmedOrders = JsonConvert.DeserializeObject<dynamic[]>(result);
                            gvConfirmedOrders.DataSource = confirmedOrders;
                            gvConfirmedOrders.DataBind();
                            lblConfirmedOrdersMessage.Text = "Confirmed orders loaded.";
                            lblConfirmedOrdersMessage.CssClass = "message message-success";
                        }
                        else
                        {
                            lblConfirmedOrdersMessage.Text = "Failed to load confirmed orders.";
                            lblConfirmedOrdersMessage.CssClass = "message message-error";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblConfirmedOrdersMessage.Text = "Error: " + ex.Message;
                lblConfirmedOrdersMessage.CssClass = "message message-error";
            }
        }


        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                // 1) Upload image if provided
                string imageUrl = string.Empty;

                if (fuProductImage.HasFile)
                {
                    using (var client = new HttpClient { BaseAddress = new Uri("https://localhost:44376/") })
                    using (var form = new MultipartFormDataContent())
                    {
                        // read file bytes
                        using (var ms = new System.IO.MemoryStream())
                        {
                            fuProductImage.PostedFile.InputStream.CopyTo(ms);
                            var bytes = ms.ToArray();

                            var fileContent = new ByteArrayContent(bytes);
                            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(fuProductImage.PostedFile.ContentType);

                            // "file" must match server side (we used FirstOrDefault, any name is fine, but "file" is conventional)
                            form.Add(fileContent, "file", System.IO.Path.GetFileName(fuProductImage.FileName));

                            var uploadResp = client.PostAsync("api/Products/UploadImage", form).Result;
                            if (!uploadResp.IsSuccessStatusCode)
                            {
                                lblProductAddMessage.Text = "Image upload failed.";
                                lblProductAddMessage.CssClass = "message message-error";
                                return;
                            }

                            var uploadJson = uploadResp.Content.ReadAsStringAsync().Result;
                            dynamic uploadResult = Newtonsoft.Json.JsonConvert.DeserializeObject(uploadJson);
                            imageUrl = (string)uploadResult.url;
                        }
                    }
                }

                // 2) Create product (include imageUrl if we have one)
                var product = new Product
                {
                    Name = txtProductName.Text.Trim(),
                    Description = txtProductDescription.Text.Trim(),
                    Category = ddlProductCategory.SelectedValue,
                    ImageUrl = imageUrl // may be empty if no file selected
                };

                using (var client = new HttpClient { BaseAddress = new Uri("https://localhost:44376/") })
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync("api/Products", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        lblProductAddMessage.Text = "Product added successfully!";
                        lblProductAddMessage.CssClass = "message message-success";

                        // Clear input fields
                        txtProductName.Text = "";
                        txtProductDescription.Text = "";
                        ddlProductCategory.SelectedIndex = 0;
                    }
                    else
                    {
                        lblProductAddMessage.Text = "Failed to add product.";
                        lblProductAddMessage.CssClass = "message message-error";
                    }
                }
            }
            catch (Exception ex)
            {
                lblProductAddMessage.Text = "Error: " + ex.Message;
                lblProductAddMessage.CssClass = "message message-error";
            }
        }



        private void LoadPendingRequests()
        {
            try
            {
                if (Session["DistributorId"] != null)
                {
                    int distributorId = (int)Session["DistributorId"];
                    using (var client = new HttpClient { BaseAddress = new Uri("https://localhost:44376/") })
                    {
                        var response = client.GetAsync($"api/Orders/PendingRequestsForDistributor/{distributorId}").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            var orders = JsonConvert.DeserializeObject<Order[]>(result);
                            gvPendingOrders.DataSource = orders;
                            gvPendingOrders.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadPendingRequests: Exception - " + ex.Message);
            }
        }

        private void LoadConfirmedOrders()
        {
            try
            {
                if (Session["DistributorId"] != null)
                {
                    int distributorId = (int)Session["DistributorId"];
                    using (var client = new HttpClient { BaseAddress = new Uri("https://localhost:44376/") })
                    {
                        var response = client.GetAsync($"api/Orders/Confirmed/{distributorId}").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            var orders = JsonConvert.DeserializeObject<dynamic[]>(result);
                            gvConfirmedOrders.DataSource = orders;
                            gvConfirmedOrders.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadConfirmedOrders: Exception - " + ex.Message);
            }
        }

        protected void gvPendingOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvPendingOrders.SelectedRow != null)
            {
                lblSelectedOrderId.Text = "Selected Order ID: " + gvPendingOrders.SelectedRow.Cells[1].Text;
                lblSelectedOrderId.Visible = true;
            }
        }

        protected void btnDistributorLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Default.aspx", false);
        }




        protected void btnSubmitQuote_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse order ID from label
                string text = lblSelectedOrderId.Text;
                int orderId;

                if (text.Contains(":"))
                {
                    string numericPart = text.Split(':')[1].Trim();
                    if (!int.TryParse(numericPart, out orderId))
                    {
                        lblQuoteMessage.Text = "Invalid Order ID format.";
                        return;
                    }
                }
                else
                {
                    lblQuoteMessage.Text = "Order ID not selected.";
                    return;
                }

                // Parse and validate price
                if (string.IsNullOrEmpty(txtQuotePrice.Text))
                {
                    lblQuoteMessage.Text = "Please enter a quote price.";
                    return;
                }

                if (!decimal.TryParse(txtQuotePrice.Text, out decimal price) || price <= 0)
                {
                    lblQuoteMessage.Text = "Please enter a valid positive quote price.";
                    return;
                }

                int distributorId = (int)Session["DistributorId"];

                // Build quote object
                var quote = new
                {
                    OrderId = orderId,
                    Price = price,
                    DistributorId = distributorId
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(quote);
                System.Diagnostics.Debug.WriteLine("Quote Payload: " + json);

                using (var client = new HttpClient { BaseAddress = new Uri("https://localhost:44376/") })
                {
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync("api/Orders/SubmitQuote", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        lblQuoteMessage.Text = "Quote submitted successfully!";
                        lblQuoteMessage.ForeColor = System.Drawing.Color.Green;

                        // Refresh grids
                        LoadPendingRequests();
                        LoadConfirmedOrders();

                        txtQuotePrice.Text = "";
                        lblSelectedOrderId.Visible = false;
                    }
                    else
                    {
                        string error = response.Content.ReadAsStringAsync().Result;
                        lblQuoteMessage.Text = "Error submitting quote: " + error;
                        System.Diagnostics.Debug.WriteLine("SubmitQuote API ERROR: " + error);
                    }
                }
            }
            catch (Exception ex)
            {
                lblQuoteMessage.Text = "Unexpected Error: " + ex.Message;
                System.Diagnostics.Debug.WriteLine("btnSubmitQuote_Click: Exception - " + ex.Message);
            }
        }

    }

    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? CustomerId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}