using BudgetControl.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BudgetControl
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

    
        }

        void Session_Start(object sender, EventArgs e)
        {
            //ForTest
            // your code here, it will be executed upon session start

            //SessionItems sessions = new SessionItems();
            //sessions.IsAuthen = true;
            //sessions.UserName = "504798";
            //sessions.TitleName = "นาย";
            //sessions.FirstName = "ปัญญา";
            //sessions.LastName = "ภิกษาวงศ์";
            //sessions.CostCenterCode = "H301023000";
            //sessions.CostCenterName = "ผสบ.กรท.";
            //SessionManager.RegisteredSession(sessions);
        }
    }
}
