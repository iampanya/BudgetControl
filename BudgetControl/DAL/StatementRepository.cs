using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BudgetControl.Models;
using BudgetControl.Models.Base;

namespace BudgetControl.DAL
{
    public class StatementRepository : IRepository<Statement>, IDisposable
    {
        private BudgetContext context;

        #region Constructor

        public StatementRepository()
        {
            context = new BudgetContext();
        }

        public StatementRepository(BudgetContext context)
        {
            this.context = context;
        }

        #endregion


        #region Additional Method

        public IEnumerable<Statement> GetPreviousStatement(Statement statement)
        {
            return Get()
                .Where(s =>
                    s.Payment.Sequence < statement.Payment.Sequence &&
                    s.BudgetID == statement.BudgetID);
        }

        public decimal GetPreviousAmount(Statement statement)
        {
            decimal previousAmount = 0;
            var previousStatement = GetPreviousStatement(statement).ToList();
            previousStatement.ForEach(t => previousAmount += t.WithdrawAmount);
            return previousAmount;
        }

        public IEnumerable<Statement> GetByPayment(Guid paymentid)
        {
            List<Statement> statements = Get()
                .Where(s => s.PaymentID == paymentid)
                .OrderBy(s => s.Budget.AccountID)
                .ToList();
            statements.ForEach(s =>
            {
                s.PreviousAmount = GetPreviousAmount(s);
                s.RemainAmount = s.Budget.BudgetAmount - s.PreviousAmount - s.WithdrawAmount;
            });

            return statements;

            //return Get()
            //    .Where(s => s.PaymentID == paymentid);
        }

        public IEnumerable<Statement> GetByBudget(Guid budgetid)
        {
            return Get()
                .Where(s => s.BudgetID == budgetid)
                .OrderBy(s => s.Payment.Sequence);
        }

        #endregion


        public IEnumerable<Statement> Get()
        {
            return context.Statements
                .Include(s => s.Budget)
                .Include(s => s.Budget.Account)
                .Include(s => s.Budget.CostCenter)
                .Include(s => s.Payment)
                .Include(s => s.Payment.Controller)
                .Include(s => s.Payment.Requester)
                .Where(s => s.Status == RecordStatus.Active && s.Payment.Status == RecordStatus.Active)
                .AsNoTracking();
        }

        public void Add(Statement entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Statement entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Statement entity)
        {
            throw new NotImplementedException();
        }

        public Statement GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        #region Implement Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        public void AddOrUpdate(Statement entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Statement> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}