using BudgetControl.Manager;
using BudgetControl.Models;
using BudgetControl.Sessions;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class ApiController : Controller
    {
        ReturnObject returnobj = new ReturnObject();

        // GET: Api
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Budget(string costcenterid = "")
        {
            try
            {
                BudgetManager bm = new BudgetManager();
                CostCenter working = AuthManager.GetWorkingCostCenter();

                //TODO check authorize
                var budgets = bm.GetByCostCenterID(costcenterid).ToList();
                var vms = bm.ConvertToVMs(budgets);
                returnobj.SetSuccess(vms);
            }
            catch (Exception ex)
            {
                returnobj.SetError("พบข้อผิดพลาด");
            }

            return Content(returnobj.ToJson(), "application/json");
        }
    }
}