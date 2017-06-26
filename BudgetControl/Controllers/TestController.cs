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

        public ActionResult Budgets()
        {
            try
            {
                CostCenter working = AuthManager.GetWorkingCostCenter();
                jsonResult.SetSuccess(_bgManager.GetByCostCenter(working));


                BudgetContext db = new BudgetContext();
                



            }
            catch (Exception ex)
            {
                jsonResult.SetError(ex.Message);
            }

            return Content(jsonResult.ToJson(), "application/json");
        }



        #endregion

    }
}