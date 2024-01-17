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
using System.IO;
using System.Text;

namespace BudgetControl.Controllers
{
    public class DataController : Controller
    {
        private ReturnObject returnobj = new ReturnObject();
        private ReportManager _reportManger = new ReportManager();

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
        private ActionResult Payments(string year, string costcenterid)
        {
            try
            {
                CostCenter working;
                List<PaymentViewModel> payments;
                PaymentManager pManager = new PaymentManager();

                // 1. Get working costcenter.
                working = AuthManager.GetWorkingCostCenter();

                // 2. Get Payment data
                payments = pManager.GetOverall(year, costcenterid).ToList();
               
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
        public ActionResult Payment(string id, string year, string costcenterid, string type)
        {
            Payment payment;
            List<BudgetTransaction> transactions = new List<BudgetTransaction>();
            PaymentManager pManager = new PaymentManager();

            // 1. If id is null or empty, then return all payments.
            if (String.IsNullOrEmpty(id))
            {
                return Payments(year, costcenterid);
            }

            if(type == "vm")
            {
                PaymentViewModel vm =  pManager.GetVMPaymentByID(id);
                returnobj.SetSuccess(vm);
            }
            else
            {
                PaymentViewModel vm = pManager.GetVMPaymentByID(id);
                payment = new Payment(vm);
                //payment = pManager.GetRawPaymentByID(id);
                returnobj.SetSuccess(payment);
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
                var result = paymentManager.Add(payment);
                returnobj.SetSuccess(result.PaymentID);
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");

        }

        [HttpPut]
        [ActionName("Payment")]
        public ActionResult UpdatePayment(Payment payment)
        {
            try
            {
                PaymentManager paymentManager = new PaymentManager();
                paymentManager.Update(payment);
                returnobj.SetSuccess(payment.PaymentID);
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpDelete]
        [ActionName("Payment")]
        public ActionResult DeletePayment(string id)
        {
            var _db = new BudgetContext();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.Database.ExecuteSqlCommand(string.Format("DELETE FROM BudgetTransaction WHERE PaymentID = '{0}'", id));
                    _db.Database.ExecuteSqlCommand(string.Format("DELETE FROM Payment WHERE PaymentID = '{0}'", id));
                    transaction.Commit();
                    returnobj.SetSuccess(true);
                    //var paymentRepo = new PaymentRepository(_db);
                    //var payment = paymentRepo.GetById(id);
                    //List<BudgetTransaction> budgetTransaction;

                    //if (payment == null)
                    //{
                    //    throw new Exception("ไม่พบข้อมูลการจ่ายเงิน");
                    //}
                    //payment.Status = RecordStatus.Remove;
                    //paymentRepo.Update(payment);
                    //paymentRepo.Save();

                    //if (payment.BudgetTransactions != null)
                    //{
                    //    var transRepo = new TransactionRepository(_db);
                    //    budgetTransaction = payment.BudgetTransactions.ToList();
                    //    budgetTransaction.ForEach(t =>
                    //    {
                    //        t.Status = RecordStatus.Remove;
                    //        transRepo.Update(t);
                    //    });
                    //    transRepo.Save();
                    //}
                    //transaction.Commit();
                    //returnobj.SetSuccess(payment);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    returnobj.SetError(ex.Message);
                }
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpGet]
        public ActionResult PaymentTransaction(string year, string costcenterid)
        {
            try
            {
                CostCenter working;
                List<PaymentTransactionViewModel> vms;
                PaymentManager pManager = new PaymentManager();

                // 1. Get working costcenter.
                working = AuthManager.GetWorkingCostCenter();

                // 2. Get Payment data
                vms = pManager.GetPaymentTransaction(year, costcenterid).ToList();

                // 3. Set Return result
                returnobj.SetSuccess(vms);

            }
            catch (Exception ex)
            {

                returnobj.SetError(ex.Message);
            }

            // 3. Return to client
            return Content(Utility.ParseToJson(returnobj), "application/json");
        }


        #endregion

        #region Budget

        [HttpGet]
        public ActionResult Budgets(string year, string costcenterid)
        {
            try
            {
                CostCenter working;
                List<Budget> budgets;

                // 1. Get working costcenter.
                working = AuthManager.GetWorkingCostCenter();

                // 2. Get by raw query
                var bm = new BudgetManager();
                budgets = bm.GetOverall(year, costcenterid).ToList();

                // 3. Set return object.
                returnobj.SetSuccess(budgets);

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");

        }

        [HttpGet]
        public ActionResult Budget(string id, string year, string costcenterid)
        {
            if (id == null)
            {
                return Budgets(year, costcenterid);
            }


            try
            {
                Budget budget;
                using (BudgetRepository budgetRep = new BudgetRepository())
                {
                    budget = budgetRep.GetById(id);
                    if (budget == null)
                    {
                        throw new Exception("ไม่พบข้อมูลงบประมาณที่เลือก");
                    }
                }

                //budget.BudgetTransactions = budget.BudgetTransactions.Where(t => t.Status == RecordStatus.Active).OrderBy(t => t.CreatedAt).ToList();

                //decimal previousAmount = 0;
                //decimal total = 0;
                //foreach (var item in budget.BudgetTransactions)
                //{
                //    using (var paymentRepo = new PaymentRepository())
                //    {
                //        item.Payment = paymentRepo.GetById(item.PaymentID);
                //    }
                //    total += item.Amount;
                //    item.PreviousAmount = previousAmount;
                //    item.RemainAmount = budget.BudgetAmount - item.PreviousAmount - item.Amount;

                //    previousAmount = item.Amount + item.PreviousAmount;
                //}
                //budget.WithdrawAmount = total;
                //budget.RemainAmount = budget.BudgetAmount - budget.WithdrawAmount;

                var bm = new BudgetManager();
                budget = bm.GetWithTransaction(id);

                returnobj.SetSuccess(budget);
            }
            catch (Exception ex)
            {
                returnobj = new ReturnObject(false, ex.Message, null);
            }

            return Content(Utility.ParseToJson(returnobj), "application/json");

        }

        [HttpPost]
        public ActionResult Budget(CreateBudgetModel form)
        {
            try
            {
                // 1. Validate account.
                Account account;
                using (var accRepo = new AccountRepository())
                {
                    account = accRepo.GetById(form.AccountID);
                    // if not then, Add to database
                    if (account == null)
                    {
                        account = new Account()
                        {
                            AccountID = form.AccountID,
                            AccountName = form.AccountName,
                            Status = RecordStatus.Active
                        };
                        accRepo.Add(account);
                        accRepo.Save();
                    }
                }

                // 2. Check Budget.
                Budget budget;
                using (var budgetRepo = new BudgetRepository())
                {
                    // 2.1 check budget is already in database
                    budget = budgetRepo.Get().Where(
                            b =>
                                b.Year == form.Year &&
                                b.AccountID == form.AccountID &&
                                b.CostCenterID == form.CostCenterID &&
                                b.Status == BudgetStatus.Active
                        ).FirstOrDefault();

                    if (budget != null)
                    {
                        throw new Exception("งบประมาณนี้มีอยู่แล้วในระบบ");
                    }
                    else // Add new budget
                    {
                        budget = new Budget()
                        {
                            BudgetID = Guid.NewGuid(),
                            AccountID = form.AccountID,
                            CostCenterID = form.CostCenterID,
                            Sequence = 0,
                            Year = form.Year,
                            BudgetAmount = form.Amount,
                            WithdrawAmount = 0,
                            RemainAmount = 0,
                            Status = BudgetStatus.Active
                        };

                        budgetRepo.Add(budget);
                        budgetRepo.Save();
                    }
                }

                // 3. Set return value
                returnobj.SetSuccess(budget);

            }
            catch (Exception ex)
            {

                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpPut]
        [ActionName("Budget")]
        public ActionResult UpdateBudget(Budget budget)
        {
            try
            {
                var budgetManager = new BudgetManager();
                budget = new Budget(budget);
                budgetManager.Update(budget);
                returnobj.SetSuccess(budget);
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpDelete]
        [ActionName("Budget")]
        public ActionResult DeleteBudget(string id)
        {
            try
            {
                using (var budgetRepo = new BudgetRepository())
                {
                    budgetRepo.Delete(id);
                    budgetRepo.Save();
                }
                returnobj.SetSuccess("delete success");
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }
            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpGet]
        public ActionResult UploadBudget()
        {
            returnobj.SetSuccess("test");
            return Content(Utility.ParseToJson(returnobj), "application/json");
        }

        //[HttpPost]
        //public ActionResult UploadBudget(List<BudgetFileModel> filedata)
        //{
        //    List<Account> accounts = new List<Account>();
        //    List<Budget> budgets = new List<Budget>();
        //    List<BudgetTransaction> transactions = new List<BudgetTransaction>();

        //    try
        //    {
        //        // 1. Validate POST parameter
        //        if (filedata == null)
        //        {
        //            throw new ArgumentNullException("filedata");
        //        }

        //        // 2. Read each row and process data.
        //        filedata = filedata.OrderBy(f => f.CostCenterID).ThenBy(f => f.AccountID).ToList();
        //        foreach (var row in filedata)
        //        {
        //            // 2.1 Check existing and add to Accounts.
        //            var account = accounts.Where(a => a.AccountID == row.AccountID).FirstOrDefault();
        //            if (account == null)
        //            {
        //                account = new Account(row);
        //                accounts.Add(account);
        //            }
        //            accounts.Add(account);

        //            // 2.2 Check existing and add to Budgets Header.
        //            var budget = budgets.Where(b =>
        //                    b.AccountID == row.AccountID &&
        //                    b.CostCenterID == row.CostCenterID &&
        //                    b.Year == row.Year
        //                ).FirstOrDefault();

        //            if (budget == null)
        //            {
        //                budget = new Budget(row);
        //                budgets.Add(budget);
        //            }

        //            // 2.3 Add to Budget Transaction.
        //            var transaction = new BudgetTransaction(row, budget);
        //            transaction.SetAmount(budgets); // Get previous amount and update budget amount
        //            transactions.Add(transaction);
        //        }

        //        // 3. Save to database
        //        using (var context = new BudgetContext())
        //        {
        //            using (var db_transaction = context.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    var accRepo = new AccountRepository(context);
        //                    var budgetRepo = new BudgetRepository(context);
        //                    var transRepo = new TransactionRepository(context);

        //                    accounts.ForEach(a => accRepo.AddOrUpdate(a));
        //                    accRepo.Save();

        //                    budgets.ForEach(b => budgetRepo.AddOrUpdate(b));
        //                    budgetRepo.Save();

        //                    transactions.ForEach(t => transRepo.Add(t));
        //                    transRepo.Save();

        //                    db_transaction.Commit();
        //                    returnobj.SetSuccess(filedata);
        //                }
        //                catch (Exception ex)
        //                {
        //                    db_transaction.Rollback();
        //                    throw ex;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        returnobj.SetError(ex.Message);
        //    }

        //    return Content(returnobj.ToJson(), "application/json");
        //}

        [HttpPost]
        public ActionResult UploadBudget_Old(List<BudgetFileModel> filedata)
        {
            List<Account> accounts = new List<Account>();
            List<Budget> budgets = new List<Budget>();
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
                    //accounts.Add(account);

                    // 2.2 Check existing and add to Budgets Header.
                    //TODO: Check existing in db
                    var budget = budgets.Where(b =>
                            b.AccountID == row.AccountID &&
                            b.CostCenterID == row.CostCenterID &&
                            b.Year == row.Year
                        ).FirstOrDefault();

                    if (budget == null)
                    {
                        // Add new budget
                        budget = new Budget(row);
                        budgets.Add(budget);
                    }
                    else
                    {
                        // Sum amount to exist budget
                        int index = budgets.IndexOf(budget);
                        budgets[index].BudgetAmount += row.Amount;
                        //budgets[index].RemainAmount += row.Amount;
                    }
                }

                //// Write to file
                //StringBuilder csvFile = new StringBuilder();
                //foreach (var budget in budgets)
                //{
                //    csvFile.Append(budget.AccountID + "," + budget.CostCenterID + "," + budget.BudgetAmount);
                //    csvFile.Append(Environment.NewLine);
                //}

                //System.IO.File.WriteAllText(@"C:/budget.txt", csvFile.ToString());

                //3.Save to database
                using (var context = new BudgetContext())
                {
                    using (var db_transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var accRepo = new AccountRepository(context);
                            var budgetRepo = new BudgetRepository(context);

                            accounts.ForEach(a => accRepo.AddOrUpdate(a));
                            accRepo.Save();

                            budgets.ForEach(b => budgetRepo.AddOrUpdate(b));
                            budgetRepo.Save();

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
                throw;
            }
            return Content(returnobj.ToJson(), "application/json");
        }


        [HttpPost]
        public ActionResult ReadBudgetFile_Old(UploadBudgetModel formdata)
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

                //budgetfile = budgetfile
                //    .Where(b => b.CostCenterID.StartsWith("H3010") || b.CostCenterID == "H301000000")
                //    .ToList();

                if (!String.IsNullOrEmpty(formdata.CostCenterBegin))
                {
                    if (String.IsNullOrEmpty(formdata.CostCenterEnd)) { formdata.CostCenterEnd = formdata.CostCenterBegin; }
                    budgetfile = budgetfile
                        .Where(b => b.CostCenterID.CompareTo(formdata.CostCenterBegin) >= 0 && b.CostCenterID.CompareTo(formdata.CostCenterEnd) <= 0)
                        .ToList();
                }

                returnobj.SetSuccess(budgetfile);

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }


            return Content(returnobj.ToJson(), "application/json");
        }

        [HttpPost]
        public ActionResult UploadBudget(List<BudgetFileModel> filedata)
        {
            List<Account> accounts = new List<Account>();
            List<Budget> budgets = new List<Budget>();

            try
            {
                var manager = new UploadManager();
                var row = manager.UpsertBudget("admin");
                returnobj.SetSuccess("นำเข้าข้อมูลสำเร็จ: " + row.ToString() + " รายการ");
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
            List<BudgetFileModel> budgetfile = new List<BudgetFileModel>();
            var manager = new UploadManager();
            budgetfile = manager.ReadBudgetFile(formdata);

            returnobj.SetSuccess(budgetfile);
            return Content(returnobj.ToJson(), "application/json");
        }

        #endregion

        #region Employee

        [HttpGet]
        private ActionResult Employees(string retireYear = "")
        {
            try
            {
                using (EmployeeRepository empRep = new EmployeeRepository())
                {
                    List<EmployeeViewModel> empviewmodel = new List<EmployeeViewModel>();
                    var emp = empRep.Get().ToList();


                    if (String.IsNullOrEmpty(retireYear))
                    {
                        emp = emp.Where(e => e.StatusCode != "D").ToList();
                    }
                    else
                    {
                        int iRetireYear;
                        List<String> retiredList = new List<string>();

                        // check parameter 
                        if(Int32.TryParse(retireYear, out iRetireYear))
                        {
                            emp.ForEach(e =>
                            {
                                if (!String.IsNullOrEmpty(e.RetiredDate))
                                {
                                    var retiredDate = e.RetiredDate.Trim();
                                    if (retiredDate.Length == 10)
                                    {
                                        int year;
                                        if(Int32.TryParse(retiredDate.Substring(retiredDate.Length - 4, 4), out year))
                                        {
                                            // Remove Employee retired 
                                            if(iRetireYear > year)
                                            {
                                                retiredList.Add(e.EmployeeID);
                                            }
                                        }
                                        
                                    }
                                }
                            });
                        }

                        emp.RemoveAll(e => retiredList.Contains(e.EmployeeID));

                    }

                    emp.ForEach(
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
        public ActionResult Employee(string id, string retireYear = "")
        {
            if (id == null)
            {
                return Employees(retireYear);
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

        #region Report
        public ActionResult SummaryReport(string year)
        {
            try
            {
                List<Budget> budgets;

                if (String.IsNullOrEmpty(year))
                {
                    year = (DateTime.Today.Year + 543).ToString();
                }

                // 1. Get working costcenter.
                CostCenter working = AuthManager.GetWorkingCostCenter();
                Employee employee = AuthManager.GetEmployeeInfo();


                int deptsap;

                using (var db = new BudgetContext())
                {
                    deptsap = db.DepartmentInfos.Where(c => c.CostCenterCode == working.CostCenterID).Select(c => c.DeptSap).FirstOrDefault();
                }
                //if (employee.DepartmentSap == null)
                //{
                //    IdmManager idm = new IdmManager();
                //    Int32.TryParse(idm.GetEmployeeProfile(AuthManager.GetCurrentUser().EmployeeID).DepartmentSap, out deptsap);
                //}
                //else
                //{
                //    deptsap = (int)employee.DepartmentSap;
                //}



                // 2. Get budget by query
                budgets = _reportManger.SummaryReport(deptsap, year).ToList();
                returnobj.SetSuccess(budgets);

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "applicaton/json");
        }


        public ActionResult Individual(string id, string year)
        {
            try
            {
                List<Budget> budgets;
                Employee employee;

                if (String.IsNullOrEmpty(year))
                {
                    year = (DateTime.Today.Year + 543).ToString();
                }

                using (var empRepo = new EmployeeRepository())
                {

                    employee = empRepo.GetById(id);
                    if (employee == null)
                    {
                        throw new Exception("ไม่พบรายชื่อพนักงาน");
                    }
                }

                budgets = _reportManger.Individual(employee, year).ToList();

                returnobj.SetSuccess(budgets);

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }


        #endregion

        #region CostCenter
        public ActionResult CostCenter(string id)
        {
            try
            {
                if (id == null)
                {
                    return CostCenters();
                }

                CostCenter costcenter;
                using (var ccRepo = new CostCenterRepository())
                {
                    costcenter = ccRepo.GetById(id);
                }

                returnobj.SetSuccess(costcenter);

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }
            return Content(returnobj.ToJson(), "application/json");
        }

        public ActionResult CostCenters()
        {
            try
            {
                List<CostCenter> costcenters;
                CostCenter working = AuthManager.GetWorkingCostCenter();
                Employee employee = AuthManager.GetEmployeeInfo();
                CostCenterManager ccaManager = new CostCenterManager();

                costcenters = ccaManager.GetWithChildren(working.CostCenterID);

                //int deptsap;
                //DepartmentInfo deptInfo;
                //using (var db = new BudgetContext())
                //{
                //    deptInfo = db.DepartmentInfos.FirstOrDefault(d => d.CostCenterCode == working.CostCenterID);
                //}
                
                //if(deptInfo == null)
                //{
                //    using (var ccRepo = new CostCenterRepository())
                //    {
                //        costcenters = ccRepo.Get()
                //            .Where(c =>
                //                c.CostCenterID.StartsWith(working.CostCenterTrim) &&
                //                c.Status == RecordStatus.Active
                //            ).OrderBy(c => c.CostCenterID).ToList();
                //    }

                //}
                //else
                //{
                //    deptsap = deptInfo.DeptSap;
                //    costcenters = ccaManager.GetWithChildren(working.CostCenterID);
                //}

                returnobj.SetSuccess(costcenters);

                //using (var ccRepo = new CostCenterRepository())
                //{
                //    costcenters = ccRepo.Get()
                //        .Where(c =>
                //            c.CostCenterID.StartsWith(working.CostCenterTrim) &&
                //            c.Status == RecordStatus.Active
                //        ).OrderBy(c => c.CostCenterID).ToList();
                //}
                //
            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }
            return Content(returnobj.ToJson(), "application/json");
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

        #region Contractor

        [HttpGet]
        public ActionResult Contractor()
        {
            try
            {
                CostCenter working;

                // 1. Get working costcenter.
                working = AuthManager.GetWorkingCostCenter();

                // 2. Get Contractor list by costcenterid
                ContractorManager ctManager = new ContractorManager();
                var contractors = ctManager.GetByCostCenterID(working.CostCenterID);

                // 3. Set return object.
                returnobj.SetSuccess(contractors);

            }
            catch (Exception ex)
            {
                returnobj.SetError(ex.Message);
            }

            return Content(returnobj.ToJson(), "application/json");
        }


        [HttpPost]
        public ActionResult Contractor(string costcenterid, string name)
        {
            try
            {
                //CostCenter working;
                // 1. Get working costcenter.
                //working = AuthManager.GetWorkingCostCenter();
                //TODO : check costcenterid with workingcostcenter

                // 2. Get Contractor list by costcenterid
                ContractorManager ctManager = new ContractorManager();
                var contractors = ctManager.Add(costcenterid, name);
                ctManager.Save();

                // 3. Set return object.
                returnobj.SetSuccess(contractors);

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