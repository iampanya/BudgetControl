using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.DAL
{
    public class TransactionRepository : IRepository<BudgetTransaction>, IDisposable
    {
        private BudgetContext _db;

        #region Constructor

        public TransactionRepository()
        {

        }

        public TransactionRepository(BudgetContext context)
        {
            this._db = context;
        }

        #endregion


        #region IRepository

        public IEnumerable<BudgetTransaction> Get()
        {
            throw new NotImplementedException();
        }

        public void Add(BudgetTransaction entity)
        {
            entity.NewCreateTimeStamp();
            _db.BudgetTransactions.Add(entity);
        }

        public void AddOrUpdate(BudgetTransaction entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(BudgetTransaction entity)
        {
            throw new NotImplementedException();
        }

        public void Update(BudgetTransaction entity)
        {
            throw new NotImplementedException();
        }

        public BudgetTransaction GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        #endregion

        #region Implement Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
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

        public IEnumerable<BudgetTransaction> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}