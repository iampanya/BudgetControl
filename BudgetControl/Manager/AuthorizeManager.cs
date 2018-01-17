using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class AuthorizeManager
    {
        private BudgetContext _db;

        #region Constructor

        public AuthorizeManager()
        {
            _db = new BudgetContext();
        }

        #endregion


        #region Read

        public List<AuthorizeCostCenter> GetByCostCenter(string costcentercode)
        {
            List<AuthorizeCostCenter> authorizes = new List<AuthorizeCostCenter>();

            authorizes = _db.AuthorizeCostCenters.Where(a => a.CostCenterCode == costcentercode).ToList();

            return authorizes;
        }

        #endregion  
    }
}