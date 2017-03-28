using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public IEnumerable<BudgetTransaction> GetAll()
        {
            return _db.BudgetTransactions
                .AsNoTracking()
                .Include(t => t.Budget)
                .Include(t => t.Budget.Account)
                .Include(t => t.Budget.CostCenter)
                .Include(t => t.Payment)
                .Include(t => t.Reference);
        }

        public IEnumerable<BudgetTransaction> Get()
        {
            return GetAll().Where(t => t.Status == RecordStatus.Active);
        }

        public void Add(BudgetTransaction entity)
        {
            // 1. Validate entity
            entity.Validate();

            // 2. Mark timestamp
            entity.NewCreateTimeStamp();

            // 3. Add to context
            _db.BudgetTransactions.Add(entity);
        }

        public void AddOrUpdate(BudgetTransaction entity)
        {
            if (GetById(entity.BudgetTransactionID) == null)
            {
                Add(entity);
            }
            else
            {
                Update(entity);
            }
        }


        public void Delete(object id)
        {
            this.Delete(this.GetById(id));
        }

        public void Delete(BudgetTransaction entity)
        {
            entity.Status = RecordStatus.Remove;
        }

        public void Update(BudgetTransaction entity)
        {
            // 1. Validate
            entity.Validate();

            // 2. Mark timestamp
            entity.NewModifyTimeStamp();

            // 3. Add to context
            _db.Entry(entity).State = EntityState.Modified;
            _db.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            _db.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        }

        public BudgetTransaction GetById(object id)
        {
            Guid transactionid;
            try
            {
                transactionid = new Guid(id.ToString());
            }
            catch (Exception)
            {
                return null;
            }
            return GetAll().FirstOrDefault(p => p.BudgetTransactionID == transactionid);
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


    }
}