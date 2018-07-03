using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class WorkingCCController : ApiController
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
        public ActionResult AddWorkingCC()
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
    }
}
