using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class BudgetTransactionManager
    {
        private BudgetContext _db;

        #region Constructor

        public BudgetTransactionManager()
        {
            _db = new BudgetContext();
        }

        public BudgetTransactionManager(BudgetContext context)
        {
            this._db = context;
        }

        #endregion

        #region Properties

        public BudgetTransaction BudgetTransaction { get; set; }

        #endregion

        #region Methods
        public Decimal GetTotalTransaction(Guid budgetid, Guid paymentid)
        {
            List<BudgetTransaction> transactions = new List<BudgetTransaction>();
            Decimal summary = 0;
            using (var tranRepo = new TransactionRepository())
            {
                transactions = tranRepo
                    .Get()
                    .Where(t => t.BudgetID == budgetid && t.PaymentID == paymentid)
                    .ToList();
            }

            foreach (var item in transactions)
            {
                summary += item.Amount;
            }

            return summary;
        }


        public void Add(BudgetTransaction transaction)
        {
            if(_db.Database.CurrentTransaction == null)
            {
                using (var dbTransaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        this.WrapAdd(transaction);
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        throw ex;
                    }
                }
            }
            else
            {
                this.WrapAdd(transaction);
            }

        }

        private void WrapAdd(BudgetTransaction transaction)
        {

            // 1. Get budget infomation
            BudgetManager budgetManager = new BudgetManager(_db);
            Budget budget = new Budget(budgetManager.Get(transaction.BudgetID));

            // 1. Initial transaction data
            this.BudgetTransaction = new BudgetTransaction(transaction);
            this.BudgetTransaction.PrepareTransactionToSave(budget);
            

            // 2. Add transaction to context
            var tranRepo = new TransactionRepository(_db);
            tranRepo.Add(this.BudgetTransaction);

            // 3. Save
            tranRepo.Save();

            /// Update budget
            /// 
            budget.WithdrawAmount -= transaction.Amount;
            budget.RemainAmount += transaction.Amount;
            budgetManager.Update(budget);

        }

        public void Update(BudgetTransaction transaction)
        {

        }

        

        public void Delete(BudgetTransaction transaction)
        {

        }


        public List<BudgetTransaction> SumTransaction(List<BudgetTransaction> trans)
        {
            List<BudgetTransaction> result = new List<BudgetTransaction>();

            foreach( var item in trans)
            {
                int i = result.IndexOf(result.FirstOrDefault(r => r.BudgetID == item.BudgetID));

                if(i > -1)
                {
                    item.Amount = item.Amount + result[i].Amount;
                    result[i] = item;
                    //result[i].Amount += item.Amount;
                }
                else
                {
                    result.Add(item);
                }
            }

            result.ForEach(r => r.Amount = Math.Abs(r.Amount));
            return result;
        }
        #endregion

    }
}
