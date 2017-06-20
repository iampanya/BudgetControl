using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BudgetControl.DAL
{
    public class AccountRepository : IRepository<Account>, IDisposable
    {
        private BudgetContext context;


        #region Constructor
        public AccountRepository()
        {
            context = new BudgetContext();
        }

        public AccountRepository(BudgetContext context)
        {
            this.context = context;
        }

        #endregion
        public IEnumerable<Account> GetAll()
        {
            return context.Accounts.AsNoTracking();
        }
        public IEnumerable<Account> Get()
        {
            return context.Accounts.AsNoTracking();
        }

        public void Add(Account entity)
        {
            entity.NewCreateTimeStamp();
            context.Accounts.Add(entity);
        }

        public void AddOrUpdate(Account entity)
        {
            if (GetById(entity.AccountID) == null)
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
            var account = context.Accounts.Find(id);

            if (account == null)
            {
                throw new Exception("Not found data");
            }
            else
            {
                Delete(account);
            }
        }

        public void Delete(Account entity)
        {
            entity.Status = RecordStatus.Inactive;
            Update(entity);
        }

        public void Update(Account entity)
        {
            entity.NewModifyTimeStamp();
            context.Accounts.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        }

        public Account GetById(object id)
        {
            return Get().Where(a => a.AccountID == id.ToString()).FirstOrDefault();
        }

        public void Save()
        {
            context.SaveChanges();
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


       
    }
}