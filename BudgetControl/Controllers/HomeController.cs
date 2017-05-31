using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class HomeController : Controller
    {
        #region Pages

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Error401()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Notice()
        {
            return View();
        }

        public ActionResult Document()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        #endregion

    }
}