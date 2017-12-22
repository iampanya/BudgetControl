using BudgetControl.IdmEmployeeServices;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class EmployeeViewModel
    {
        #region Constructor

        public EmployeeViewModel()
        {

        }

        public EmployeeViewModel(EmployeeProfile emp)
        {
            EmployeeID = emp.EmployeeId.TrimStart(new char[] { '0' });
            TitleName = emp.TitleFullName;
            FirstName = emp.FirstName;
            LastName = emp.LastName;
            if(emp.PositionDescShort == emp.LevelDesc)
            {
                JobTitle = emp.PositionDescShort;
            }
            else
            {
                JobTitle = emp.PositionDescShort + emp.LevelDesc;
            }
            CostCenterID = emp.CostCenterCode;
            CostCenterName = emp.CostCenterName;
        }

        public EmployeeViewModel(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException();
            }
            EmployeeID = employee.EmployeeID;
            TitleName = employee.TitleName;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            JobTitle = employee.JobTitle;
            CostCenterID = employee.CostCenterID;
            CostCenterName = employee.CostCenter.ShortName;
        }

        #endregion

        #region Fields

        public string EmployeeID { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }
        public string Description
        {
            get
            {
                return TitleName + " " + FirstName + "  " + LastName;
            }
        }

        #endregion
    }
}