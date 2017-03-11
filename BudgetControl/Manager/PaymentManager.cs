using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class PaymentManager
    {
        private BudgetContext _db;

        #region Constructor

        public PaymentManager()
        {
            _db = new BudgetContext();
        }

        public PaymentManager(BudgetContext context)
        {
            this._db = context;
        }

        #endregion

        #region Properties

        public Payment Payment { get; set; }
      
        #endregion

        #region Methods

        public void Add(Payment payment)
        {
            // 1. Add Payment


            // 2. Add all transaction to payment
        }

        public void Update(Payment payment)
        {

        }

        public void Delete(Payment payment)
        {

        }


        #endregion

    }
}