using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetControl.Manager
{
    public class TransactionManager
    {
        private BudgetContext _db;
        private GenericRepository<BudgetTransaction> _transRepo;

        #region Constructor

        public TransactionManager()
        {
            _db = new BudgetContext();
            _transRepo = new GenericRepository<BudgetTransaction>(_db);
        }

        public TransactionManager(BudgetContext db)
        {
            _db = db;
            _transRepo = new GenericRepository<BudgetTransaction>(_db);
        }

	    #endregion

        #region Create



        #endregion

        #region Read

        public IEnumerable<BudgetTransaction> GetAll()
        {
            return _transRepo.GetAll();
        }

        public IEnumerable<BudgetTransaction> GetByBudget(Guid budgetid)
        {
            // 1. Get budget data
            BudgetManager bm = new BudgetManager();
            Budget budget = bm.GetByID(budgetid);
            
            return GetByBudget(budget);
        }

        public IEnumerable<BudgetTransaction> GetByBudget(Budget budget)
        {
            // 1. Get transaction data
            List<BudgetTransaction> transactions = _transRepo
                .Get(t => t.BudgetID == budget.BudgetID && t.Status == RecordStatus.Active,  
                        o => o.OrderBy(t => t.RowVersion), 
                        "Payment")
                .ToList();

            // 2. Calculate transaction amount
            decimal withdraw = 0; // initial withdraw amount
            foreach(var item in transactions)
            {
                item.Budget = budget; 
                item.PreviousAmount = withdraw;
                item.RemainAmount = budget.BudgetAmount - item.PreviousAmount - item.Amount;

                withdraw = withdraw + item.Amount;                
            }

            return transactions;

        }



        #endregion

        #region Update



        #endregion

        #region Delete



        #endregion
    }
}