using System;
using GadgetHubClient.Services;

namespace GadgetHubClient
{
    public partial class Login : System.Web.UI.Page // Keep the existing class name for now
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var service = new ProductServices();
                var result = service.LoginDistributor(txtEmail.Text, txtPassword.Text);

                if (result != null && result.DistributorId != null)
                {
                    Session["DistributorId"] = (int)result.DistributorId;
                    Response.Redirect("distributor.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid email or password.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Login error: {ex.Message}";
            }
        }
    }
}