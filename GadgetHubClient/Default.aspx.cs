using System;

namespace GadgetHubClient
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void btnCustomerLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerLogin.aspx");
        }

        protected void btnDistributorLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("DistributorLogin.aspx");
        }
    }
}