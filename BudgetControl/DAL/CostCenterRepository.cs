using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.DAL
{
    public class CostCenterRepository : IRepository<CostCenter>, IDisposable
    {
        private BudgetContext context;

        #region Constructor
        public CostCenterRepository()
        {
            context = new BudgetContext();
        }

        public CostCenterRepository(BudgetContext context)
        {
            this.context = context;
        }

        #endregion

        #region Repository

        public IEnumerable<CostCenter> GetAll()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CostCenter> Get()
        {
            throw new NotImplementedException();
        }

        public void Add(CostCenter entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(CostCenter entity)
        {
            throw new NotImplementedException();
        }

        public void Update(CostCenter entity)
        {
            throw new NotImplementedException();
        }

        public CostCenter GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
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


        public void AddOrUpdate(CostCenter entity)
        {
            throw new NotImplementedException();
        }
    }
}