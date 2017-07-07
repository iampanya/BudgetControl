using BudgetControl.DAL;
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
    public class TestController : Controller
    {
        private BudgetManager _bgManager;
        private ReturnObject jsonResult = new ReturnObject();

        #region Constructor

        public TestController()
        {
            _bgManager = new BudgetManager();
        }

        #endregion

        #region Budget

        public ActionResult Budgets(string id, string costcenterid)
        {
            // if id is not empty then, get by budgetid
            if (!String.IsNullOrEmpty(id))
            {
                return Budget(id);
            }

            // else get budget by costcenter 
            try
            {
                //CostCenter working = AuthManager.GetWorkingCostCenter();
                if (String.IsNullOrEmpty(costcenterid))
                {
                    costcenterid = "H301023010";
                }

                BudgetManager bm = new BudgetManager();
                var result = bm.GetSummaryBudget(costcenterid);
                jsonResult.SetSuccess(result);

            }
            catch (Exception ex)
            {
                jsonResult.SetError(ex.Message);
            }

            return Content(jsonResult.ToJson(), "application/json");
        }

        private ActionResult Budget(string id)
        {

            try
            {
                BudgetManager bm = new BudgetManager();
                var result = bm.GetBudgetDetail(new Guid(id));
                jsonResult.SetSuccess(result);
            }
            catch (Exception ex)
            {
                jsonResult.SetError(ex.Message);
            }

            return Content(jsonResult.ToJson(), "application/json");
        }



        #endregion


        #region View

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}