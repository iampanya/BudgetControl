using BudgetControl.Models.BudgetMT;
using BudgetControl.Sessions;
using BudgetControl.Util;
using BudgetControl.ViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetControl.Controllers
{
    public class BudgetMTController : Controller
    {
        // GET: MTBudget
        private string conn_string = ConfigurationManager.ConnectionStrings["BudgetContext"].ConnectionString;


        private ReturnObject returnobj = new ReturnObject();
        public ActionResult RequestView()
        {
            return View();
        }

        public ActionResult Transaction()
        {
            return View();
        }

        #region API
        [HttpGet]
        public ActionResult GetRequestById(Guid id)
        {
            try
            {
                string ownerCostcenter = AuthManager.GetAuthentication().Employee.CostCenterID;
                using (var conn = new SqlConnection(conn_string))
                {
                    conn.Open();

                    var result = conn.QueryFirstOrDefault<BudgetMTTransaction>("[sp_BudgetMT_Transaction_GetById]"
                        , new
                        {
                            @Id = id,
                        }
                        , commandType: System.Data.CommandType.StoredProcedure
                    );
                    result.InputSeminarDate = result.SeminarDate?.ToString("dd/MM/yyyy");
                    returnobj.SetSuccess(result);
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }


            return Content(returnobj.ToJson(), "application/json");
        }

        public ActionResult Request(string year)
        {
            try
            {
                string ownerCostcenter = AuthManager.GetAuthentication().Employee.CostCenterID;
                using (var conn = new SqlConnection(conn_string))
                {
                    conn.Open();

                    var result = conn.Query<BudgetMTTransaction>("sp_BudgetMT_Transaction_Get"
                        , new
                        {
                            @Year = year,
                            @CostCenter = ownerCostcenter
                        }
                        , commandType: System.Data.CommandType.StoredProcedure
                    );
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
        public ActionResult Request(BudgetMTTransaction formData)
        {
            try
            {
                formData.CreatedBy = AuthManager.GetCurrentUser().EmployeeID;
                formData.OwnerCostCenter = AuthManager.GetAuthentication().Employee.CostCenterID;
                int mealCount = (formData.HasMealMorning ? 1 : 0) + (formData.HasMealAfternoon ? 1 : 0);
                int mealPrice = 35;
                formData.TotalAmount = formData.ParticipantCount * mealPrice * mealCount;
                formData.SeminarDate = formData.InputSeminarDate.ToDate(); 
                using (var conn = new SqlConnection(conn_string))
                {
                    conn.Open();

                    var result = conn.QueryFirstOrDefault<BudgetMTTransaction>("sp_BudgetMT_Transaction_Insert"
                        , new 
                        {
                            @Year = formData.Year,
                            @Title = formData.Title,
                            @OwnerDepartment = formData.OwnerDepartment,
                            @OwnerCostCenter = formData.OwnerCostCenter,
                            @Participant = formData.Participant,
                            @SeminarDate = formData.SeminarDate,
                            @Location = formData.Location,
                            @ParticipantCount = formData.ParticipantCount,
                            @HasMealMorning = formData.HasMealMorning,
                            @HasMealAfternoon = formData.HasMealAfternoon,
                            @Remark = formData.Remark,
                            @TotalAmount = formData.TotalAmount,
                            @RemainAmount = formData.RemainAmount,
                            @CreatedBy = formData.CreatedBy,
                        }
                        ,commandType: System.Data.CommandType.StoredProcedure
                    );
                    returnobj.SetSuccess(result);
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpDelete]
        public ActionResult Request(Guid id)
        {
            try
            {
                string deleteBy = AuthManager.GetCurrentUser().UserName;
                using (var conn = new SqlConnection(conn_string))
                {
                    conn.Open();

                    var result = conn.QueryFirstOrDefault<BudgetMTTransaction>("sp_BudgetMT_Transaction_Delete"
                        , new
                        {
                            @Id = id,
                            @DeleteBy = deleteBy
                        }
                        , commandType: System.Data.CommandType.StoredProcedure
                    );
                    returnobj.SetSuccess(result);
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }


            return Content(returnobj.ToJson(), "application/json");
        }


        [HttpPut]
        [ActionName("Request")]
        public ActionResult UpdateRequest(BudgetMTTransaction formData)
        {
            try
            {
                formData.ModifiedBy = AuthManager.GetCurrentUser().EmployeeID;
                //formData.OwnerCostCenter = AuthManager.GetAuthentication().Employee.CostCenterID;
                int mealCount = (formData.HasMealMorning ? 1 : 0) + (formData.HasMealAfternoon ? 1 : 0);
                int mealPrice = 35;
                formData.TotalAmount = formData.ParticipantCount * mealPrice * mealCount;

                formData.SeminarDate = formData.InputSeminarDate.ToDate();

                using (var conn = new SqlConnection(conn_string))
                {
                    conn.Open();

                    var result = conn.QueryFirstOrDefault<BudgetMTTransaction>("sp_BudgetMT_Transaction_Update"
                        , new
                        {
                            @Id = formData.Id,
                            @Year = formData.Year,
                            @Title = formData.Title,
                            @OwnerDepartment = formData.OwnerDepartment,
                            @OwnerCostCenter = formData.OwnerCostCenter,
                            @Participant = formData.Participant,
                            @SeminarDate = formData.SeminarDate,
                            @Location = formData.Location,
                            @ParticipantCount = formData.ParticipantCount,
                            @HasMealMorning = formData.HasMealMorning,
                            @HasMealAfternoon = formData.HasMealAfternoon,
                            @Remark = formData.Remark,
                            @TotalAmount = formData.TotalAmount,
                            @RemainAmount = formData.RemainAmount,
                            @ModifiedBy = formData.ModifiedBy
                        }
                        , commandType: System.Data.CommandType.StoredProcedure
                    );
                    returnobj.SetSuccess(result);
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        #endregion
    }
}