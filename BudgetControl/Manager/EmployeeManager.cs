using BudgetControl.DAL;
using BudgetControl.IdmEmployeeServices;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class EmployeeManager
    {
        private BudgetContext _db;

        #region Constructor

        public EmployeeManager()
        {
            _db = new BudgetContext();
        }

        public EmployeeManager(BudgetContext context)
        {
            _db = context;
        }

        #endregion


        #region Update

        public Employee UpdateFromIDM(EmployeeProfile empProfile)
        {
            Employee employee;
            try
            {
                string employeeid = empProfile.EmployeeId.TrimStart(new char[] { '0' });

                // 1. Get employee from database
                var employeeInDb = _db.Employees
                    .Where(e => e.EmployeeID == employeeid && e.Status == RecordStatus.Active)
                    .FirstOrDefault();

                // 2. Convert from EmployeeProfile to Employee
                employee = new Employee(empProfile);

                if (employeeInDb == null)
                {
                    // Add new
                    employee.NewCreateTimeStamp();
                    _db.Entry(employee).State = System.Data.Entity.EntityState.Added;
                    _db.SaveChanges();
                }
                else
                {
                    // Check has change
                    if (employeeInDb.HasChange(employee))
                    {
                        //Update
                        employeeInDb.FirstName = employee.FirstName;
                        employeeInDb.LastName = employee.LastName;
                        employeeInDb.JobTitle = employee.JobTitle;
                        employeeInDb.JobLevel = employee.JobLevel;
                        employeeInDb.CostCenterID = employee.CostCenterID;
                        employeeInDb.Status = RecordStatus.Active;
                        employeeInDb.NewModifyTimeStamp();
                        _db.Entry(employeeInDb).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                }

                return employee;
            }

            catch (Exception ex)
            {
                throw new Exception("Error : ไม่สามารถอัพเดทข้อมูลพนักงานได้");
            }
        }

        #endregion
    }
}