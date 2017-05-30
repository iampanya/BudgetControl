using BudgetControl.DAL;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class BudgetsController : Controller
    {
        private BudgetContext db = new BudgetContext();

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

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #endregion

    }
}
