using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Sessions;
using BudgetControl.Util;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class UserController : Controller
    {
        private ReturnObject returnobj = new ReturnObject();
        // GET: Account

        #region View

        public ActionResult Index()
        {
            return View();
        }

        // Deprecated
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Signin()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ChangeWorkingCostCenter()
        {
            return View();
        }

        #endregion

        #region Login Action

        [HttpPost]
        public ActionResult Login(LoginViewModel logindata)
        {
            try
            {
                AuthManager.Login(logindata);
                returnobj.SetSuccess(AuthManager.GetAuthentication());
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        #endregion

        #region Logout Action

        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                AuthManager.Logout();
                returnobj.SetSuccess("Logout Success.");
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }
            return Content(returnobj.ToJson(), "application/json");
        }

        #endregion

        #region ACTION: Change Password

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel form)
        {
            
            try
            {
                // 0. Validate user input
                form.Validate();

                // 1. Get data from User
                using (var userRepo = new UserRepository())
                {
                    var user = userRepo.Get()
                        .Where(
                            u =>
                                u.UserName == form.Username &&
                                u.Password == form.Password)
                        .FirstOrDefault();
                    
                    if (user == null)
                    {
                        throw new Exception("รหัสผ่านเดิมไม่ถูกต้อง");
                    }

                    // 2. Change Password
                    user.Password = form.NewPassword;
                    userRepo.Update(user);

                    // 3. Return
                    returnobj.SetSuccess("เปลี่ยนรหัสผ่านสำเร็จ");
                }

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }
            return Content(returnobj.ToJson(), "application/json");
        }

        #endregion

        #region Check User Authorize

        public ActionResult GetAuthen()
        {

            // Reorder command
            // 1. Get Authen first 
            // 2. if not found then execute loginbytoken()
            // 3. return session
            try
            {
                AuthManager.LoginByToken();
                returnobj.SetSuccess(AuthManager.GetAuthentication());
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }
            return Content(returnobj.ToJson(), "application/json");
        }

        #endregion

        #region Change Working Costcenter

        [HttpPost]
        public ActionResult ChangeCostCenter(string costcenterid)
        {
            try
            {
                using (var db = new BudgetContext())
                {
                    var costcenter = db.CostCenters.Where(c => c.CostCenterID == costcenterid).FirstOrDefault();
                    AuthManager.ChangeWorkingCostcenter(costcenter);

                    // Save history to database
                    CurrentWorkingCC current = new CurrentWorkingCC();
                    current.Id = Guid.NewGuid();
                    current.WorkingCostCenterID = costcenter.CostCenterID;
                    current.EmployeeID = AuthManager.GetCurrentUser().EmployeeID;
                    current.NewCreateTimeStamp();
                    db.CurrentWorkingCCs.Add(current);
                    db.SaveChanges();

                    returnobj.SetSuccess(AuthManager.GetAuthentication());
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        #endregion


        [ActionName("Current")]
        public ActionResult CurrentUser()
        {
            try
            {
                using (var userRepo = new UserRepository())
                {
                    var user = userRepo.Get()
                        .Where(u => u.UserName == AuthManager.GetCurrentUser().UserName)
                        .FirstOrDefault();
                    if (user == null)
                    {
                        throw new Exception("Not found.");
                    }
                    returnobj.SetSuccess(user);
                    return Content(returnobj.ToJson(), "application/json");
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
                return Content(returnobj.ToJson(), "application/json");
            }


        }
    }
}