using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Sessions
{
    public class SessionItems
    {
        #region Constructor

        public SessionItems()
        {

        }

        public SessionItems(User user)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }
            this.Role = user.Role;
            this.User = user;
            this.Employee = this.User.Employee;
            this.WorkingCostCenter = user.Employee.CostCenter;
            
        }

        #endregion
        //public bool IsAuthen { get; set; }
        //public bool IsAdmin { get; set; }
        //public string UserName { get; set; }
        //public string EmployeeCode { get; set; }
        //public string TitleName { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string CostCenterCode { get; set; }
        //public string CostCenterName { get; set; }
        //public string BaCode { get; set; }

        public UserRole Role { get; set; }
        public User User { get; set; }
        public Employee Employee { get; set; }
        public DepartmentInfo WorkingDept { get; set; }
        public CostCenter WorkingCostCenter { get; set; }

    }
}