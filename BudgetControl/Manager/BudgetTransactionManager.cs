using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class BudgetTransactionManager
    {
        private BudgetContext _db;

        #region Constructor

        public BudgetTransactionManager()
        {
            _db = new BudgetContext();
        }

        public BudgetTransactionManager(BudgetContext context)
        {
            this._db = context;
        }

        #endregion

        #region Properties

        public BudgetTransaction BudgetTransaction { get; set; }

        #endregion

        #region Methods

        public void Add(BudgetTransaction transaction)
        {

        }

        public void Update(BudgetTransaction transaction)
        {

        }

        public void Delete(BudgetTransaction transaction)
        {

        }


        #endregion

    }
}
}