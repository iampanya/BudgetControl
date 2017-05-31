using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class BudgetsController : Controller
    {

        #region Pages

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Upload() 
        {
            return View();
        }
        
        public ActionResult ConfirmDelete()
        {
            return View();
        }

        #endregion

    }
}
