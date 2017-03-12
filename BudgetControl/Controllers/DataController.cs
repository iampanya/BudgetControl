using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Sessions;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Newtonsoft.Json;
using BudgetControl.Util;
using BudgetControl.Models.Base;
using BudgetControl.Manager;

namespace BudgetControl.Controllers
{
    public class DataController : Controller
    {
        private ReturnObject returnobj = new ReturnObject();

        //// GET: Data

        #region Controller // Deprecate
        [HttpGet]
        public ActionResult Controller(string id, string paymentid)
        {

            if (!String.IsNullOrEmpty(id))
            {
                return Employee(id);
            }

            if (!String.IsNullOrEmpty(paymentid))
            {
                throw new NotImplementedException();
            }

            return Employee(AuthManager.GetCurrentUser().EmployeeID);
        }


        #endregion

        #region Payment

        [HttpGet]
        private ActionResult Payments()
        {
            try
            {
                CostCenter working;
                List<Payment> payments;

                // 1. Get working costcenter.
                working = AuthManager.GetWorkingCostCenter();

                // 2. Get payment data.
                using (PaymentRepository paymentRepo = new PaymentRepository())
                {
                    payments = paymentRepo.Get()
                        .Where(
                            p =>
                                p.CostCenterID.StartsWith(working.CostCenterTrim)
                        )
                        .ToList();

                }
                // 3. Set Return result
                returnobj.SetSuccess(payments);

            }
            catch (Exception ex)
            {

                returnobj.SetError(ex.Message);
            }

            // 3. Return to client
            return Content(Utility.ParseToJson(returnobj), "application/json");

        }

        [HttpGet]
        public ActionResult Payment(string id)
        {
            // 1. If id is null or empty, then return all payments.
            if (String.IsNullOrEmpty(id))
            {
                return Payments();
            }

            // 2. Get payment by id
            try
            {
                using (PaymentRepository paymentRepo = new PaymentRepository())
                {
                    Payment payment = paymentRepo.GetById(id);
                    if (payment == null)
                    {
                        throw new Exception("ไม่พบข้อมูลงบประมาณที่เลือก");
                    }
                    //TODO Get transaction details
                    returnobj.SetSuccess(payment);
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }
            // 3. Return to client
            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpGet]
        public ActionResult EmptyPayment()
        {
            returnobj.SetSuccess(new PaymentViewModel());
            return Content(Utility.ParseToJson(returnobj), "application/json");
        }

        [HttpPost]
        public ActionResult Payment(Payment payment)
        {
            try
            {
                PaymentManager paymentManager = new PaymentManager();
                paymentManager.Add(payment);
                returnobj.SetSuccess("Create Payment success.");
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");

        //try
        //{
        //    using (PaymentRepository paymentRep = new PaymentRepository())
        //    {
        //        Payment payment = new Payment(paymentviewmodel);

        //        ////Create New Payment
        //        if (paymentviewmodel.PaymentID == Guid.Empty)
        //        {
        //            payment.PaymentID = Guid.NewGuid();
        //            paymentRep.Add(payment);
        //        }
        //        //Update Exist Payment
        //        else
        //        {
        //            paymentRep.Update(payment);
        //        }

        //        returnobj.SetSuccess(payment.PaymentID);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    returnobj.SetError(ex.Message);
        //}

        //return Content(Utility.ParseToJson(returnobj), "application/json");
    }

        [HttpPut]
        [ActionName("Payment")]
        public ActionResult UpdatePayment(Payment payment)
        {
            
            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpDelete]
        [ActionName("Payment")]
        public ActionResult DeletePayment(string id)
        {
            try
            {
                using (PaymentRepository paymentRepo = new PaymentRepository())
                {
                    paymentRepo.Delete(id);
                    returnobj.SetSuccess(id);
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(Utility.ParseToJson(returnobj), "application/json");
        }



        #endregion

        #region Budget

        [HttpGet]
        public ActionResult Budgets()
        {
            try
            {
                CostCenter working;
                List<Budget> budgets;

                // 1. Get working costcenter.
                working = AuthManager.GetWorkingCostCenter();

                // 2. Get budget data.
                using (BudgetRepository budgetRep = new BudgetRepository())
                {
                    budgets = budgetRep.Get().ToList();
                    budgets = budgetRep.Get()
                        .Where(
                            b =>
                                b.CostCenterID.StartsWith(working.CostCenterTrim)
                        )
                        .ToList();
                }

                // 3. Set return object.
                returnobj.SetSuccess(budgets);

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");

            //try
            //{
            //    CostCenter working = AuthManager.GetWorkingCostCenter();

            //    using (BudgetRepository budgetRep = new BudgetRepository())
            //    {
            //        List<BudgetViewModel> budgetviewmodels = new List<BudgetViewModel>();
            //        budgetRep.Get().ToList().ForEach(
            //            b => budgetviewmodels.Add(new BudgetViewModel(b)));
            //        returnobj = new ReturnObject(true, string.Empty, budgetviewmodels);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    returnobj = new ReturnObject(false, ex.Message, null);
            //}

            //return Content(Utility.ParseToJson(returnobj), "application/json");
        }

        [HttpGet]
        public ActionResult Budget(string id)
        {
            if (id == null)
            {
                return Budgets();
            }

            try
            {
                using (BudgetRepository budgetRep = new BudgetRepository())
                {
                    Budget budget = budgetRep.GetById(id);
                    if (budget == null)
                    {
                        throw new Exception("ไม่พบข้อมูลงบประมาณที่เลือก");
                    }
                    returnobj.SetSuccess(budget);
                }
            }
            catch (Exception ex)
            {
                returnobj = new ReturnObject(false, ex.Message, null);
            }

            return Content(Utility.ParseToJson(returnobj), "application/json");

            //if (id == null)
            //{
            //    return Budgets();
            //}

            //try
            //{
            //    using (BudgetRepository budgetRep = new BudgetRepository())
            //    {
            //        BudgetViewModel budgetviewmodel = new BudgetViewModel(budgetRep.GetById(id));
            //        budgetviewmodel.GetDetails();
            //        returnobj = new ReturnObject(true, string.Empty, budgetviewmodel);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    returnobj = new ReturnObject(false, ex.Message, null);
            //}

            //return Content(Utility.ParseToJson(returnobj), "application/json");
        }

        [HttpPost]
        public ActionResult Budget(Budget budget)
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadBudget()
        {
            returnobj.SetSuccess("test");
            return Content(Utility.ParseToJson(returnobj), "application/json");
        }

        //[HttpPost]
        //public ActionResult UploadBudget(UploadBudgetModel budgetfile)
        //{
        //    List<Budget> budgets = new List<Budget>();
        //    List<Account> accounts = new List<Account>();

        //    try
        //    {

        //        string[] lines = budgetfile.FileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        //        foreach (var line in lines)
        //        {
        //            string[] columns = line.Split('|');
        //            if (columns.Length == 7)
        //            {
        //                if (columns[2][0] == '5')
        //                {
        //                    accounts.Add(new Account
        //                    {
        //                        AccountID = columns[2].Trim(),
        //                        AccountName = columns[3].Trim()
        //                    });

        //                    budgets.Add(new Budget
        //                    {
        //                        BudgetID = Guid.NewGuid(),
        //                        AccountID = columns[2].Trim(),
        //                        CostCenterID = columns[5].Trim(),
        //                        Year = budgetfile.Year,
        //                        BudgetAmount = float.Parse(columns[4]),
        //                        Status = BudgetStatus.Active
        //                    });
        //                }
        //            }
        //        }

        //        List<Account> distinctAccount = accounts.GroupBy(a => a.AccountID).Select(a => a.First()).ToList();

        //        RecordTimeStamp rt = new RecordTimeStamp();
        //        rt.NewTimeStamp();

        //        using (var context = new BudgetContext())
        //        {


        //            distinctAccount.ForEach(a =>
        //            {
        //                a.SetCreateTimeStamp(rt);
        //                context.Accounts.AddOrUpdate(c => c.AccountID, a);
        //            });
        //            context.SaveChanges();
        //        }


        //        using (var context = new BudgetContext())
        //        {
        //            budgets.ForEach(b =>
        //            {

        //                var budgetindb = context.Budgets
        //                    .Where(c =>
        //                        c.AccountID == b.AccountID &&
        //                        c.CostCenterID == b.CostCenterID &&
        //                        c.Year == b.Year)
        //                    .FirstOrDefault();

        //                if (budgetindb == null)
        //                {
        //                    b.SetCreateTimeStamp(rt);
        //                    context.Budgets.Add(b);
        //                }
        //                else
        //                {
        //                    budgetindb.BudgetAmount = b.BudgetAmount;
        //                    budgetindb.SetModifiedTimeStamp(rt);
        //                    context.Budgets.Attach(budgetindb);
        //                    context.Entry(budgetindb).State = EntityState.Modified;
        //                }
        //            });
        //            context.SaveChanges();

        //            returnobj.SetSuccess("Success");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        returnobj.SetError(ex.Message);
        //    }
        //    return Content(Utility.ParseToJson(returnobj), "application/json");
        //}

        [HttpPost]
        public ActionResult UploadBudget(List<BudgetFileModel> filedata)
        {
            List<Account> accounts = new List<Account>();
            List<Budget> budgets = new List<Budget>();
            List<BudgetTransaction> transactions = new List<BudgetTransaction>();

            try
            {
                // 1. Validate POST parameter
                if (filedata == null)
                {
                    throw new ArgumentNullException("filedata");
                }

                // 2. Read each row and process data.
                filedata = filedata.OrderBy(f => f.CostCenterID).ThenBy(f => f.AccountID).ToList();
                foreach (var row in filedata)
                {
                    // 2.1 Check existing and add to Accounts.
                    var account = accounts.Where(a => a.AccountID == row.AccountID).FirstOrDefault();
                    if (account == null)
                    {
                        account = new Account(row);
                        accounts.Add(account);
                    }
                    accounts.Add(account);

                    // 2.2 Check existing and add to Budgets Header.
                    var budget = budgets.Where(b =>
                            b.AccountID == row.AccountID &&
                            b.CostCenterID == row.CostCenterID &&
                            b.Year == row.Year
                        ).FirstOrDefault();

                    if (budget == null)
                    {
                        budget = new Budget(row);
                        budgets.Add(budget);
                    }

                    // 2.3 Add to Budget Transaction.
                    var transaction = new BudgetTransaction(row, budget);
                    transaction.SetAmount(budgets); // Get previous amount and update budget amount
                    transactions.Add(transaction);
                }

                // 3. Save to database
                using (var context = new BudgetContext())
                {
                    using (var db_transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var accRepo = new AccountRepository(context);
                            var budgetRepo = new BudgetRepository(context);
                            var transRepo = new TransactionRepository(context);

                            accounts.ForEach(a => accRepo.AddOrUpdate(a));
                            accRepo.Save();

                            budgets.ForEach(b => budgetRepo.AddOrUpdate(b));
                            budgetRepo.Save();

                            transactions.ForEach(t => transRepo.Add(t));
                            transRepo.Save();

                            db_transaction.Commit();
                            returnobj.SetSuccess(filedata);
                        }
                        catch (Exception ex)
                        {
                            db_transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpPost]
        public ActionResult ReadBudgetFile(UploadBudgetModel formdata)
        {
            List<Account> accounts = new List<Account>();
            List<Budget> budgets = new List<Budget>();
            List<BudgetTransaction> transactions = new List<BudgetTransaction>();
            List<BudgetFileModel> budgetfile = new List<BudgetFileModel>();

            try
            {
                formdata.Validate();

                // 1. Read file line by line
                string[] lines = formdata.FileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (var line in lines)
                {
                    // 1.1 Split each line in to columns
                    string[] columns = line.Split('|');

                    // 1.2 Validate data line
                    if (columns.Length == 6)
                    {
                        // Check second column is digit
                        if (Char.IsDigit(columns[1].Trim()[0]))
                        {
                            // This is data line
                            budgetfile.Add(new BudgetFileModel(columns, formdata.Year));
                        }
                    }
                }

                //Remove data for test
                //budgetfile = budgetfile.Where(b => b.CostCenterID == AuthManager.GetWorkingCostCenter().CostCenterID).ToList();

                budgetfile = budgetfile
                    .Where(b => b.CostCenterID.StartsWith("H301023"))
                    .ToList();

                returnobj.SetSuccess(budgetfile);

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }


            return Content(returnobj.ToJson(), "application/json");
        }

        #endregion

        #region Employee

        [HttpGet]
        private ActionResult Employees()
        {
            try
            {
                using (EmployeeRepository empRep = new EmployeeRepository())
                {
                    List<EmployeeViewModel> empviewmodel = new List<EmployeeViewModel>();
                    empRep.Get().ToList().ForEach(
                        e => empviewmodel.Add(new EmployeeViewModel(e))
                    );
                    returnobj = new ReturnObject(true, "", empviewmodel);
                }
            }
            catch (Exception ex)
            {
                returnobj = new ReturnObject(false, ex.Message, null);
            }

            return Content(Utility.ParseToJson(returnobj), "application/json");
        }

        [HttpGet]
        public ActionResult Employee(string id)
        {
            if (id == null)
            {
                return Employees();
            }

            try
            {
                using (EmployeeRepository empRep = new EmployeeRepository())
                {
                    EmployeeViewModel empviewmodel = new EmployeeViewModel(empRep.GetById(id));
                    returnobj = new ReturnObject(true, "", empviewmodel);
                }
            }
            catch (Exception ex)
            {
                returnobj = new ReturnObject(false, ex.Message, null);
            }

            return Content(Utility.ParseToJson(returnobj), "application/json");
        }

        [HttpPost]
        public ActionResult Employee(Employee employee)
        {
            return View();
        }


        #endregion

        #region Temp

        //public ActionResult PopulateBudget()
        //{
        //    //budgetRep.Get();
        //    var budget = budgetRep.Get().ToList();
        //    List<PopulateBudgetModel> budgetModel = new List<PopulateBudgetModel>();
        //    budget.ForEach(b =>
        //    {
        //        budgetModel.Add(new PopulateBudgetModel
        //        {
        //            BudgetID = b.BudgetID,
        //            AccountID = b.AccountID,
        //            AccountName = b.Account.AccountName,
        //            CostCenterID = b.CostCenterID,
        //            CostCenterName = b.CostCenter.ShortName,
        //            Year = b.Year,
        //            Amount = b.BudgetAmount
        //        });
        //    });

        //    return Json(budgetModel, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult GetBudgetController()
        //{
        //    string username = "504798"; //SessionManager.GetSessionUserName();
        //    Employee employee = empRep.GetById(username);
        //    EmployeeInfo empinfo = new EmployeeInfo(employee);


        //    return Json(empinfo, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetEmployeeList()
        //{
        //    string costcenterid = "H301023010";
        //    var employees = empRep.Get().Where(e => e.CostCenterID == costcenterid).ToList();
        //    List<EmployeeInfo> empList = new List<EmployeeInfo>();
        //    employees.ForEach(e => empList.Add(new EmployeeInfo(e)));
        //    return Json(empList, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetPaymentByID(string paymentid = "9C52F6B3-34EA-4993-A4CB-29A6D755FF65")
        //{
        //    using (PaymentRepository paymentRep = new PaymentRepository(false))
        //    {
        //        Payment payment = new Payment();
        //        payment.Statements = new List<Statement>();
        //        payment.Requester = new Employee();
        //        payment.Controller = new Employee();
        //        //payment = paymentRep.GetById(paymentid);

        //        var result = JsonConvert.SerializeObject(
        //            payment,
        //            Formatting.None,
        //            new JsonSerializerSettings()
        //            {
        //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //            }
        //        );
        //        return Content(result, "application/json");
        //    }
        //}

        #endregion

        #region Account
        [HttpGet]
        public ActionResult Accounts()
        {
            try
            {
                using (AccountRepository accountRep = new AccountRepository())
                {
                    returnobj.SetSuccess(accountRep.Get().ToList());
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(Utility.ParseToJson(returnobj), "application/json");
        }

        [HttpGet]
        public ActionResult Account(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return Accounts();
            }

            try
            {
                using (AccountRepository accountRep = new AccountRepository())
                {
                    returnobj.SetSuccess(accountRep.GetById(id));
                }
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }


            return Content(Utility.ParseToJson(returnobj), "application/json");
        }
        #endregion


    }
}