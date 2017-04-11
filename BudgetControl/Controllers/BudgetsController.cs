using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.ViewModels;
using Newtonsoft.Json;

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

        #region Services TEMP

        public ActionResult GetBudget()
        {
            List<BudgetViewModel> budgetViewModels = new List<BudgetViewModel>();
            List<Budget> budgets = new List<Budget>();

            using (var budgetRep = new BudgetRepository())
            {
                budgets = budgetRep.Get().ToList();
                budgets.ForEach(b => budgetViewModels.Add(new BudgetViewModel(b)));
            }

            var result = JsonConvert.SerializeObject(
                    budgetViewModels,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
            return Content(result, "application/json");
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
