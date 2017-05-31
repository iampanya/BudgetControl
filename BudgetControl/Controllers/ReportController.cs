using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class ReportController : Controller
    {
        #region Pages
        
        public ActionResult Summary()
        {
            return View();
        }

        public ActionResult Individual()
        {
            return View();
        }

        #endregion
    }
}