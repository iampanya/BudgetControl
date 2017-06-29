using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class TransactionViewModel
    {

        #region Constructor

        public TransactionViewModel()
        {

        }

        #endregion

        #region Fields

        public string BudgetTransactionID { get; set; }
        public string BudgetID { get; set; }
        public string PaymentID { get; set; }

        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public string Status { get; set; }

        public RecordTimeStamp Timestamp { get; set; }

        #endregion

    }
}