using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BudgetControl.ViewModels;
using BudgetControl.DAL;

namespace BudgetControl.Models
{
    public enum TransactionType
    {
        //Define budget amout
        Definition = 0x01, 

        //Transaction
        Transaction = 0x02
    }
    public class BudgetTransaction : RecordTimeStamp
    {
        #region Constructor

        public BudgetTransaction()
        {

        }

        // Define Budget
        public BudgetTransaction(BudgetFileModel budgetfile, Budget budget)
        {
            this.BudgetTransactionID = Guid.NewGuid();
            this.Description = "";
            this.Amount = budgetfile.Amount;
            this.Type = TransactionType.Definition;

            //Budget
            this.BudgetID = budget.BudgetID;
            this.Budget = budget;
        }


        public BudgetTransaction(BudgetTransaction tran)
        {
            this.BudgetTransactionID = tran.BudgetTransactionID;
            this.BudgetID = tran.BudgetID;
            this.PaymentID = tran.PaymentID;
            this.RowVersion = tran.RowVersion;
            this.Description = tran.Description;
            this.Amount = tran.Amount;
            this.PreviousAmount = tran.PreviousAmount;
            this.RemainAmount = tran.RemainAmount;
            this.RefID = tran.RefID;
            this.Type = tran.Type;
            this.Status = tran.Status;
        }


        #endregion

        #region Fields

        public Guid BudgetTransactionID { get; set; }
        public Guid BudgetID { get; set; }
        public Guid? PaymentID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public Guid? RefID { get; set; }
        public TransactionType Type { get; set; }
        public RecordStatus Status { get; set; }

        #endregion

        #region Relation

        [ForeignKey("RefID")]
        public virtual BudgetTransaction Reference { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Budget Budget { get; set; }

        #endregion

        #region Validate

        public void Validate()
        {

        }

        #endregion

        #region Methods
        public void SetAmount(List<Budget> budgets)
        {
            int index = budgets.IndexOf(budgets.Where(b => b.BudgetID == this.BudgetID).FirstOrDefault());
            this.PreviousAmount = budgets[index].BudgetAmount - budgets[index].RemainAmount;
            budgets[index].BudgetAmount = budgets[index].BudgetAmount + this.Amount;
            budgets[index].RemainAmount = budgets[index].RemainAmount + this.Amount;
        }

        // Prepare budget transaction before save
        public void PrepareTransactionToSave(Budget budget)
        {
            this.BudgetTransactionID = Guid.NewGuid();
            this.PreviousAmount = budget.WithdrawAmount;
            this.RemainAmount = budget.RemainAmount + this.Amount;
            this.Type = TransactionType.Transaction;
            this.NewCreateTimeStamp();
        }

        public void PrepareTransactionToSave()
        {
            this.BudgetTransactionID = Guid.NewGuid();
            this.Type = TransactionType.Transaction;
            this.NewCreateTimeStamp();
        }


        #endregion
    }
}