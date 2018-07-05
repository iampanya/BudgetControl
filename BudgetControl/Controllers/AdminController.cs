using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class AdminController : Controller
    {
        #region Page

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageWorkingCC()
        {
            return View();
        }

        public ActionResult AddWorkingCC()
        {
            return View();
        }

        #endregion
    }
}
