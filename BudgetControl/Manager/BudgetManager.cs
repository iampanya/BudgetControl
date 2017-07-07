using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetControl.Manager
{
    public class BudgetManager
    {

        private BudgetContext _db;
        private GenericRepository<Budget> _budgetRepo;

        #region Constructor

        public BudgetManager()
        {
            _db = new BudgetContext();
        }

        public BudgetManager(BudgetContext context)
        {
            _db = context;
            _budgetRepo = new GenericRepository<Budget>(_db);
        }

        #endregion

        #region Properties

        public Budget Budget { get; set; }

        #endregion

        #region Methods

        public Budget Get(Guid budgetid)
        {
            using (var budgetRepo = new BudgetRepository())
            {
                return budgetRepo.GetById(budgetid);
            }
        }

        public void Add(Budget budget)
        {
            // 1. Add Payment


            // 2. Add all transaction to payment
        }

        public void Update(Budget budget)
        {
            BudgetRepository budgetRepo = new BudgetRepository(_db);
            budget.NewModifyTimeStamp();
            budgetRepo.Update(budget);
            budgetRepo.Save();
        }

        //public void Delete(Budget budget)
        //{
           
        //}

        #endregion

        #region Create

        // Change to Add() later
        public void AddBudget(Budget budget)
        {
            // 1. Validate entity
            this.Validate(budget);

            // 2. Set new timestamp
            budget.NewCreateTimeStamp();

            // 3. Add budget to database
            _budgetRepo.Add(budget);
            _budgetRepo.Save();
        }

        public void AddByForm(CreateBudgetModel form)
        {
            // 1. Check AccountID is exist in database?
            AccountManager accManager = new AccountManager();
            if (accManager.GetByID(form.AccountID) == null)
            {
                accManager.Add(new Account()
                {
                    AccountID = form.AccountID,
                    AccountName = form.AccountName,
                    Status = RecordStatus.Active
                });
            }

            // 2. Check Budget is exist in database?
            if(GetBudget(form.Year, form.AccountID, form.CostCenterID) == null)
            {
                this.AddBudget(new Budget()
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
                });
            }
        }

        #endregion

        #region Read

        /**
         * Get summary budget in costcenter data
        **/
        public IEnumerable<BudgetIndexViewModel> GetSummaryBudget(string costcenterid)
        {
            
            // 1. Get budget data with related by costcenterid
            List<Budget> budgets =  _budgetRepo.Get( 
                    b => 
                        b.CostCenterID == costcenterid &&
                        b.Status == BudgetStatus.Active
                    , o => o.OrderBy(x => x.AccountID)
                    , "Account,BudgetTransactions,CostCenter", null, null)
                .ToList();


            // 2. Convert original budget to viewmodel template
            List<BudgetIndexViewModel> vmBudgets = new List<BudgetIndexViewModel>();
            foreach(var budget in budgets)
            {
                // Calculate withdraw amount
                budget.WithdrawAmount = GetBudgetWithdrawAmount(budget.BudgetTransactions.ToList());
                budget.RemainAmount = budget.BudgetAmount - budget.WithdrawAmount;

                // Convert into viewmodel
                vmBudgets.Add(new BudgetIndexViewModel(budget));
            }

            // 3. Return viewmodel
            return vmBudgets;
        }

        /**
         * Get budget and detail by budgetid
        **/
        public BudgetDetailViewModel GetBudgetDetail(Guid budgetid)
        {
            Budget budget;

            // 1. Get budget data
            budget = _budgetRepo.GetById(budgetid);

            // 2. Get Transaction data
            TransactionManager tm = new TransactionManager();
            budget.BudgetTransactions = tm.GetByBudget(budget).ToList();

            // 3. Get budget withdraw amount
            budget.WithdrawAmount = GetBudgetWithdrawAmount(budget.BudgetTransactions.ToList());
            budget.RemainAmount = budget.BudgetAmount - budget.WithdrawAmount;

            // 4. Convert into viewmodel
            BudgetDetailViewModel vm = new BudgetDetailViewModel(budget);

            // 5. Return viewmodel
            return vm;
        }


        public IEnumerable<Budget> GetAll()
        {
            return _budgetRepo.GetAll();
        }

        private IEnumerable<Budget> GetByStatus(BudgetStatus status)
        {
            return _budgetRepo.Get(b => b.Status == status);
        }

        public IEnumerable<Budget> GetActiveBudget()
        {
            return GetByStatus(BudgetStatus.Active);
        }

        public IEnumerable<Budget> GetClosedBudget()
        {
            return GetByStatus(BudgetStatus.Closed);
        }

        public IEnumerable<Budget> GetCancelledBudget()
        {
            return GetByStatus(BudgetStatus.Cancelled);
        }

        public Budget GetBudget(string year, string accountid, string costcenterid)
        {
            return GetActiveBudget()
                .Where(b =>
                   b.Year == year &&
                   b.AccountID == accountid &&
                   b.CostCenterID == costcenterid
                  ).FirstOrDefault();
        }

        public Budget GetByID(Guid budgetid)
        {
            return _budgetRepo.GetById(budgetid);
        }

        public IEnumerable<Budget> GetByCostCenter(CostCenter costcenter)
        {
            return GetAll().Where(b => b.CostCenterID.StartsWith(costcenter.CostCenterTrim));
        }

        #endregion

        #region Update

        // Change to Update() later
        public void UpdateBudget(Budget budget)
        {
            // 1. Validate entity
            this.Validate(budget);

            // 2. Modify timestamp
            budget.NewModifyTimeStamp();

            //3. Update to database
            _budgetRepo.Update(budget);
            _budgetRepo.Save();
        }

        #endregion

        #region Deactivate

        public void Deactivate(Guid budgetid)
        {
            this.Deactivate(this.GetByID(budgetid));
        }

        public void Deactivate(Budget budget)
        {
            // 1. Validate entity
            this.Validate(budget);

            // 2. Change budget status to Cancelled
            budget.Status = BudgetStatus.Cancelled;
            this.Update(budget);
        }

        #endregion

        #region Delete

        public void Delete(Guid budgetid)
        {
            this.Delete(this.GetByID(budgetid));
        }

        public void Delete(Budget budget)
        {
            //1. Validate entity
            this.Validate(budget);

            // 2. Modify timestamp
            budget.NewModifyTimeStamp();

            // 3. Delete from datatbase
            _budgetRepo.Delete(budget);
            _budgetRepo.Save();
        }

        #endregion

        #region Validate

        public void Validate(Budget budget)
        {
            // 1. Check entity is null
            if (budget == null)
            {
                throw new Exception("Budget cannot be null");
            }

            // 2. Check content inside entity
            var validateResult = budget.Validate();
            if (!validateResult.IsSuccess)
            {
                throw new Exception(validateResult.ErrorMessage);
            }
        }
        #endregion


        #region Methods

        /**
          * Calculate withdraw amount by budgetid
        **/
        private decimal GetBudgetWithdrawAmount(Guid budgetid)
        {
            //1. Get budget transaction by budgetid
            BudgetContext db = new BudgetContext();
            var transactions = db.BudgetTransactions.Where(t => t.BudgetID == budgetid && t.Status == RecordStatus.Active).ToList();

            //send budget transaction to calculate method
            return GetBudgetWithdrawAmount(transactions);
        }

        /**
         * Calculate withdraw amount by list of transaction
        **/
        private decimal GetBudgetWithdrawAmount(List<BudgetTransaction> transactions)
        {
            decimal withdrawAmount;

            // select sum(amount) where status = 'active' group by budgetid
            var query_result = transactions
                .Where(t => t.Status == RecordStatus.Active)
                .GroupBy(t => t.BudgetID)
                .Select(s => new
                {
                    Value = s.Sum(x => x.Amount)
                }).FirstOrDefault();

            withdrawAmount = query_result == null ? 0 : query_result.Value;

            return withdrawAmount;
        }

        #endregion
        
    }
}