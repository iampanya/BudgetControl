using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class BudgetManager
    {
        private BudgetContext _db;


        #region Constructor

        public BudgetManager()
        {
            _db = new BudgetContext();
        }

        public BudgetManager(BudgetContext context)
        {
            this._db = context;
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

        public void Delete(Budget budget)
        {
           
        }

        #endregion


        #region Refactor
        private GenericRepository<Budget> budgetRepo = new GenericRepository<Budget>();

        #region Get Budget

        public IEnumerable<Budget> GetAll()
        {
            return budgetRepo.Get();
        }

        private IEnumerable<Budget> GetByStatus(BudgetStatus status)
        {
            return GetAll().Where(b => b.Status == status);
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


        #endregion


        #region Add Budget
        
        public void AddBudget(Budget budget)
        {
            // Set new timestamp
            budget.NewCreateTimeStamp();

            // Add budget to database
            budgetRepo.Add(budget);
            budgetRepo.Save();
        }

        public void AddBudgetByForm(CreateBudgetModel form)
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

        #endregion
    }
}