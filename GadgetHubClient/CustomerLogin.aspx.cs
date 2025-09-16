using System;
using GadgetHub.Models;
using GadgetHubClient.Services;

namespace GadgetHubClient
{
    public partial class CustomerLogin : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var service = new ProductServices();
                var result = service.LoginCustomer(txtEmail.Text, txtPassword.Text);

                if (result != null && result.CustomerId != null)
                {
                    Session["CustomerId"] = (int)result.CustomerId;
                    Response.Redirect("products.aspx", false);
                    Context.ApplicationInstance.CompleteRequest(); // avoid ThreadAbortException
                }
                else
                {
                    lblMessage.Text = "Invalid email or password.";
                    lblMessage.CssClass = "message message-error";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Login error: {ex.Message}";
                lblMessage.CssClass = "message message-error";
            }
        }

        protected void btnRegisterSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var svc = new ProductServices();
                var newCustomer = new Customer
                {
                    Name = txtRegName.Text?.Trim(),
                    Email = txtRegEmail.Text?.Trim(),
                    PasswordHash = txtRegPassword.Text?.Trim() // hash in production
                };

                if (string.IsNullOrWhiteSpace(newCustomer.Name) ||
                    string.IsNullOrWhiteSpace(newCustomer.Email) ||
                    string.IsNullOrWhiteSpace(newCustomer.PasswordHash))
                {
                    lblRegisterMessage.Text = "Please fill in all fields.";
                    lblRegisterMessage.CssClass = "message message-error";
                    return;
                }

                if (svc.RegisterCustomer(newCustomer, out var err))
                {
                    lblRegisterMessage.Text = "Registration successful! You can now log in.";
                    lblRegisterMessage.CssClass = "message message-success";

                    // prefill
                    txtEmail.Text = newCustomer.Email;
                    txtPassword.Text = newCustomer.PasswordHash;
                }
                else
                {
                    lblRegisterMessage.Text = "Registration failed: " + (err ?? "Unknown error");
                    lblRegisterMessage.CssClass = "message message-error";
                }
            }
            catch (Exception ex)
            {
                lblRegisterMessage.Text = "Error: " + ex.Message;
                lblRegisterMessage.CssClass = "message message-error";
            }
        }
    }
}
