using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    //public class TransactionViewModel
    //{

    //    #region Constructor

    //    public TransactionViewModel()
    //    {

    //    }

    //    #endregion

    //    #region Fields

    //    public string BudgetTransactionID { get; set; }
    //    public string BudgetID { get; set; }
    //    public string PaymentID { get; set; }

    //    public string Description { get; set; }
    //    public decimal Amount { get; set; }
    //    public decimal PreviousAmount { get; set; }
    //    public decimal RemainAmount { get; set; }
    //    public string Status { get; set; }

    //    public RecordTimeStamp Timestamp { get; set; }

    //    #endregion

    //}

    public class TransactionIndexViewModel
    {

        #region Constructor

        public TransactionIndexViewModel()
        {
            Timestamp = new RecordTimeStamp();
        }

        public TransactionIndexViewModel(BudgetTransaction transaction)
        {
            Timestamp = new RecordTimeStamp();

            BudgetTransactionID = transaction.BudgetTransactionID;
            Amount = transaction.Amount;
            PreviousAmount = transaction.PreviousAmount;
            RemainAmount = transaction.RemainAmount;

            // Budget info
            BudgetID = transaction.BudgetID;
            if(transaction.Budget != null)
            {
                BudgetAmount = transaction.Budget.BudgetAmount;
            }

            // Payment info
            PaymentID = transaction.PaymentID ?? Guid.Empty;
            if(transaction.Payment != null)
            {
                Description = transaction.Payment.Description;
                PaymentNo = transaction.Payment.PaymentNo;
            }

            // Timestamp
            Timestamp.CreatedAt = transaction.CreatedAt;
            Timestamp.CreatedBy = transaction.CreatedBy;
            Timestamp.ModifiedAt = transaction.ModifiedAt;
            Timestamp.ModifiedBy = transaction.ModifiedBy;
        }

        #endregion

        #region Fields

        public Guid BudgetTransactionID { get; set; }
        public decimal Amount { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal RemainAmount { get; set; }

        // Budget Info
        public Guid BudgetID { get; set; }
        public decimal BudgetAmount { get; set; }

        // Payment Info
        public Guid PaymentID { get; set; }
        public string PaymentNo { get; set; }
        public string Description { get; set; }

        // TimeStamp
        public RecordTimeStamp Timestamp { get; set; }

        #endregion


    }


}