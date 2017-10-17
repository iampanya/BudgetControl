using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BudgetControl.Sessions;
using BudgetControl.Models.Base;

namespace BudgetControl.DAL
{
    public class BudgetRepository : IRepository<Budget>, IDisposable
    {
        private BudgetContext _db;
        //private CostCenter _workingCostCenter;

        #region Constructor
        public BudgetRepository()
        {
            _db = new BudgetContext();
            InitInstance();
        }

        public BudgetRepository(BudgetContext context)
        {
            this._db = context;
            InitInstance();
        }

        public BudgetRepository(CostCenter workingCostCenter)
        {

        }

        private void InitInstance()
        {
            //this._workingCostCenter = AuthManager.GetWorkingCostCenter();
            //if (this._workingCostCenter == null)
            //{
            //    throw new Exception("Please Login");
            //}
        }

        #endregion

        #region IRepository
        public IEnumerable<Budget> GetAll()
        {
            return _db.Budgets
                .Include(c => c.Account)
                .Include(c => c.CostCenter)
                .Include(c => c.Statements)
                .Include(c => c.BudgetTransactions)
                .OrderBy(c => c.Year)
                .ThenBy(c => c.CostCenterID)
                .ThenBy(c => c.AccountID)
                .AsNoTracking();
        }

        public IEnumerable<Budget> Get()
        {
            return GetAll().Where(a => a.Status == BudgetStatus.Active);
        }

        public void Add(Budget entity)
        {
            entity.NewCreateTimeStamp();
            _db.Budgets.Add(entity);
        }

        public void AddOrUpdate(Budget entity)
        {
            var budget = Get().FirstOrDefault(a => a.AccountID == entity.AccountID && a.Year == entity.Year && a.CostCenterID == entity.CostCenterID);
            if (budget == null)
            {
                Add(entity);
            }
            else
            {
                entity.BudgetID = budget.BudgetID;
                Update(entity);
            }
            //if (GetById(entity.BudgetID) == null)
            //{
            //    Add(entity);
            //}
            //else
            //{
            //    Update(entity);
            //}
        }

        public void Delete(object id)
        {
            var budget = GetById(id);

            if (budget == null)
            {
                throw new Exception("Not found data");
            }
            else
            {
                Delete(budget);
            }
        }

        public void Delete(Budget entity)
        {
            entity.Status = BudgetStatus.Cancelled;
            Update(entity);
        }

        public void Update(Budget entity)
        {
            entity.NewModifyTimeStamp();
            //_db.Budgets.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            _db.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            _db.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        }

        public Budget GetById(object id)
        {
            Guid budgetid;
            try
            {
                budgetid = new Guid(id.ToString());
            }
            catch (Exception)
            {
                return null;
            }
            return GetAll().FirstOrDefault(b => b.BudgetID == budgetid);
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