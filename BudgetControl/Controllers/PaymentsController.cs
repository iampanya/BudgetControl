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

namespace BudgetControl.Controllers
{
    public class PaymentsController : Controller
    {
        private BudgetContext db = new BudgetContext();

        #region Pages

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Result()
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
