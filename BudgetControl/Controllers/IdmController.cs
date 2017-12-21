﻿using BudgetControl.Manager;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class IdmController : Controller
    {

        private ReturnObject returnobj = new ReturnObject();

        // GET: Idm
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployeeProfile(string empno)
        {
            IdmManager idm = new IdmManager();
            var empProfile = idm.GetEmployeeProfile(empno);
            if (empProfile == null)
            {
                returnobj.SetError("ไม่พบข้อมูลพนักงาน");
            }
            else
            {
                returnobj.SetSuccess(new EmployeeViewModel(empProfile));
            }

            return Content(returnobj.ToJson(), "application/json");
        }
    }
}