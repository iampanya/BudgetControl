using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class DataViewModels
    {

    }

    public class PopulateBudgetModel
    {
        public Guid BudgetID { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }
        public string Year { get; set; }
        public float Amount { get; set; }

        public string Description
        {
            get
            {
                return "[" + CostCenterName + "] - " + AccountName + "";
            }
        }
    }

    public class EmployeeInfo
    {
        public EmployeeInfo(Employee employee)
        {
            EmployeeID = employee.EmployeeID;
            TitleName = employee.TitleName;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            JobTitle = employee.JobTitle;
            CostCenterID = employee.CostCenterID;
            CostCenterName = employee.CostCenter.ShortName;
        }
        public string EmployeeID { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }
        public string Description {
            get
            {
                return TitleName + " " + FirstName + "  " + LastName;
            }
        }
    }

}