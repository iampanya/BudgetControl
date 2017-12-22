using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class CostCenterManager
    {
        private BudgetContext _db;

        #region Constructor

        public CostCenterManager()
        {
            _db = new BudgetContext();
        }

        public CostCenterManager(BudgetContext context)
        {
            _db = context;
        }

        #endregion

        #region Read

        public CostCenter GetByID(string id)
        {
            return _db.CostCenters.Find(id);
        }

        #endregion


        #region Add 

        public CostCenter Add(string id, string name, string dept_name)
        {
            CostCenter costcenter = new CostCenter();
            costcenter.CostCenterID = id;
            costcenter.CostCenterName = name;
            costcenter.ShortName = dept_name;
            costcenter.Status = Models.Base.RecordStatus.Active;
            costcenter.NewCreateTimeStamp();
            _db.CostCenters.Add(costcenter);
            _db.SaveChanges();
            return costcenter;
        }

        #endregion

    }
}