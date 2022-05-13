using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class BudgetMTController : Controller
    {
        // GET: MTBudget
        public ActionResult Request()
        {
            return View();
        }

        public ActionResult Transaction()
        {
            return View();
        }
    }
}