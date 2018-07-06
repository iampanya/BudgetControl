using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Sessions;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                var currentUser = AuthManager.GetCurrentUser();
                var employeeid = currentUser.UserName;
                var costcenterid = currentUser.Employee.CostCenterID;

                using (var db = new BudgetContext())
                {
                    List<CostCenter> costcenterlist = new List<CostCenter>();
                    costcenterlist.Add(db.CostCenters.Where(c => c.CostCenterID == costcenterid).FirstOrDefault());

                    // Working CostCenter Condition
                    var workingCondition = db.WorkingCCs.Where(e => e.EmployeeNo == employeeid || e.CostCenterCode == costcenterid).ToList();
                    

                    foreach (var item in workingCondition)
                    {
                        switch (item.Condition)
                        {
                            case ConditionType.ExactMatch:
                                costcenterlist.AddRange(db.CostCenters.Where(c => c.CostCenterID == item.CCAStart).ToList());
                                break;
                            case ConditionType.Between:
                                costcenterlist.AddRange(db.CostCenters.Where(c => c.CostCenterID.CompareTo(item.CCAStart) >= 0 && c.CostCenterID.CompareTo(item.CCAEnd) <= 0).ToList());
                                break;
                            case ConditionType.Contain:
                                costcenterlist.AddRange(db.CostCenters.Where(c => c.CostCenterID.StartsWith(item.CCAStart)).ToList());
                                break;
                            default:
                                break;
                        }
                    }
                    var finallist = costcenterlist.Select(c => new { c.CostCenterID, c.CostCenterName }).Distinct().OrderBy(c => c.CostCenterID).ToList();

                    returnobj.SetSuccess(finallist);
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpGet]
        public ActionResult GetCondition(string filter)
        {
            try
            {
                using (var db = new BudgetContext())
                {
                    var result = db.WorkingCCs.Where(w => w.EmployeeNo == filter || w.CostCenterCode.ToLower() == filter.ToLower()).ToList();
                    returnobj.SetSuccess(result);
                }
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
                if (!working.IsValid())
                {
                    throw new Exception("กรุณาระบุข้อมูลให้ครบถ้วน");
                }
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

        [HttpDelete]
        public ActionResult DeleteCondition(Guid? id)
        {
            try
            {
                using (var db = new BudgetContext())
                {
                    var entity = db.WorkingCCs.Find(id);
                    db.Entry(entity).State = EntityState.Deleted;
                    db.SaveChanges();
                    returnobj.SetSuccess(entity);
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
