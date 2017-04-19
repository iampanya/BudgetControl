using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BudgetControl.Sessions;

namespace BudgetControl.DAL
{
    public class EmployeeRepository : IRepository<Employee>, IDisposable
    {
        private BudgetContext context;
        private CostCenter _workingCostCenter;

        #region Constructor

        public EmployeeRepository()
        {
            this.context = new BudgetContext();

            InitInstance();
        }

        public EmployeeRepository(BudgetContext context)
        {
            this.context = context;

            InitInstance();
        }

        private void InitInstance()
        {
            this._workingCostCenter = AuthManager.GetWorkingCostCenter();
            if (this._workingCostCenter == null)
            {
                throw new Exception("Please Login");
            }
        }

        #endregion

        #region IRepository
      
        public IEnumerable<Employee> Get()
        {
            return context.Employees
                .Include(c => c.CostCenter)
                .AsNoTracking()
                .Where(c => c.CostCenterID.StartsWith(this._workingCostCenter.CostCenterTrim));
        }


        public void Add(Employee entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Employee entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Employee entity)
        {
            throw new NotImplementedException();
        }

        public Employee GetById(object id)
        {
            if (id == null) { throw new Exception("ไม่พบรายชื่อพนักงาน"); }
            var employee = Get().FirstOrDefault(e => e.EmployeeID == id.ToString());

            if (employee == null) 
            { 
                throw new Exception("ไม่พบรายชื่อพนักงาน"); 
            }
            else 
            { 
                return employee; 
            }
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


        public void AddOrUpdate(Employee entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}