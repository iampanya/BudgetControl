using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class BudgetManager
    {
        private BudgetContext _db;

        #region Constructor

        public BudgetManager()
        {
            _db = new BudgetContext();
        }

        public BudgetManager(BudgetContext context)
        {
            this._db = context;
        }

        #endregion

        #region Properties

        public Budget Budget { get; set; }

        #endregion

        #region Methods

        public void Add(Budget budget)
        {
            // 1. Add Payment


            // 2. Add all transaction to payment
        }

        public void Update(Budget budget)
        {

        }

        public void Delete(Budget budget)
        {

        }


        #endregion
    }
}