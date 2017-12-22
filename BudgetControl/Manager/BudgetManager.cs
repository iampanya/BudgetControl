using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using BudgetControl.ViewModels;

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

        public IQueryable<Budget> GetAll()
        {
            return _db.Budgets
                    .Include(b => b.Account)
                    .Include(b => b.CostCenter)
                    .AsNoTracking();
        }

        public IQueryable<Budget> GetActive()
        {
            return GetAll()
                .Where(b => b.Status == BudgetStatus.Active);
        }

        public IEnumerable<Budget> GetByCostCenterID(string costcenterid)
        {
            return GetActive().Where(b => b.CostCenterID == costcenterid);
        }

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

        #region Convert To VM

        public BudgetViewModel ConvertToVM(Budget budget)
        {
            return new BudgetViewModel(budget);
        }

        public IEnumerable<BudgetViewModel> ConvertToVMs(List<Budget> budgets)
        {
            List<BudgetViewModel> vms = new List<BudgetViewModel>();
            foreach(var budget in budgets)
            {
                vms.Add(ConvertToVM(budget));
            }
            return vms;
        }

        #endregion

    }
}