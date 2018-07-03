using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class WorkingCCController : Controller
    {
        private ReturnObject returnobj = new ReturnObject();

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                returnobj.SetSuccess("");
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpPost]
        public ActionResult AddWorkingCC(WorkingCC working)
        {
            try
            {
                using (var db = new BudgetContext())
                {
                    working.Id = Guid.NewGuid();
                    working.NewCreateTimeStamp();
                    db.WorkingCCs.Add(working);
                    db.SaveChanges();
                    returnobj.SetSuccess(working);
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }
    }
}
