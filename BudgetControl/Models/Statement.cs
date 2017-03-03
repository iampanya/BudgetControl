using BudgetControl.Models.Base;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class Statement : RecordTimeStamp
    {

        #region Constructor
        public Statement()
        {

        }


        public Statement(Statement statement)
        {
            this.StatementID = statement.StatementID;
            this.PaymentID = statement.PaymentID;
            this.BudgetID = statement.BudgetID;
            this.Description = statement.Description;
            this.WithdrawAmount = statement.WithdrawAmount;
            this.PreviousAmount = statement.PreviousAmount;
            this.RemainAmount = statement.RemainAmount;
            this.Status = statement.Status;

            this.Payment = statement.Payment;
            this.Budget = statement.Budget;
        }
        public Statement(StatementViewModel statement)
        {
            this.StatementID = statement.StatementID;
            this.PaymentID = statement.PaymentID;
            this.BudgetID = statement.BudgetID;
            this.Description = statement.Description;
            this.WithdrawAmount = statement.WithdrawAmount;
            this.PreviousAmount = statement.PreviousAmount;
            this.RemainAmount = statement.RemainAmount;

            RecordStatus rs;
            Enum.TryParse(statement.Status, out rs);
            this.Status = rs;
        }

        #endregion
        public Guid StatementID { get; set; }

        //[Index("IX_BudgetAndPayment", 1, IsUnique = true)]
        public Guid PaymentID { get; set; }
        //[Index("IX_BudgetAndPayment", 2, IsUnique = true)]
        public Guid BudgetID { get; set; }

        public string Description { get; set; }

        public decimal WithdrawAmount { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public RecordStatus Status { get; set; }

        #region Relation
        public virtual Payment Payment { get; set; }
        public virtual Budget Budget { get; set; }

        #endregion
    }
}