﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;

namespace BudgetControl.Manager
{
    public class ContractorManager
    {
        private BudgetContext _db;

        #region Constructor

        public ContractorManager()
        {
            _db = new BudgetContext();
        }

        public ContractorManager(BudgetContext context)
        {
            _db = context;
        }

        #endregion

        #region Create

        public Contractor Add(string costcenterid, string name)
        {
            Contractor contractor = new Contractor();
            contractor.ID = Guid.NewGuid();
            contractor.Name = name;
            contractor.Status = RecordStatus.Active;
            contractor.NewCreateTimeStamp();
            _db.Entry(contractor).State = EntityState.Added;
            return contractor;
        }

        #endregion

        #region Read

        public IQueryable<Contractor> Get()
        {
            return null;
        }
        
        public IQueryable<Contractor> GetByCostCenterID()
        {
            return null;
        }

        #endregion

        #region Save

        public void Save()
        {
            _db.SaveChanges();
        }

        #endregion
    }
}