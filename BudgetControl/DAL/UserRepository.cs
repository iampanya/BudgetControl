using BudgetControl.Models;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BudgetControl.DAL
{
    public class UserRepository : IRepository<User>, IDisposable
    {
        private BudgetContext context;

        #region Constructor
        public UserRepository()
        {
            context = new BudgetContext();
        }

        #endregion


        #region Methods


        #endregion


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

        public IEnumerable<User> Get()
        {
            return context.Users
                .Include(u => u.Employee)
                .Include(u => u.Employee.CostCenter)
                .AsNoTracking();
        }

        public void Add(User entity)
        {
            context.Users.Add(entity);
            Save();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            try
            {
                context.Users.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            context.SaveChanges();
        }


        public void AddOrUpdate(User entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}