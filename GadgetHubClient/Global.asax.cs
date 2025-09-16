using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace GadgetHubClient
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Initialize application (e.g., Web API if used, but not required for Web Forms)
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Session start logic
        }
    }
}