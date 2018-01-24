using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class TransactionViewModels
    {
        #region Constructor

        public TransactionViewModels()
        {
            RecordTimeStamp = new RecordTimeStamp();
        }

        #endregion

        #region Fields

        public Guid BudgetTransactionID { get; set; }

        // Budget Infomation
        public Guid BudgetID { get; set; }
        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public decimal BudgetAmount { get; set; }
        
        // Payment Description
        public Guid? PaymentID { get; set; }
        public string Description { get; set; }

        // Amount
        public decimal Amount { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal RemainAmount { get; set; }


        public RecordStatus Status { get; set; }
        public RecordTimeStamp RecordTimeStamp { get; set; }

        #endregion
    }
}