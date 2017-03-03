using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class StatementViewModel
    {
        public StatementViewModel()
        {

        }
        public StatementViewModel(Statement statement)
        {
            this.StatementID = statement.StatementID;
            this.PaymentID = statement.PaymentID;
            this.BudgetID = statement.BudgetID;
            this.Description = statement.Description;
            this.WithdrawAmount = statement.WithdrawAmount;
            this.PreviousAmount = statement.PreviousAmount;
            this.RemainAmount = statement.RemainAmount;
            this.Status = statement.Status.ToString();

            //Budget info
            this.BudgetYear = statement.Budget.Year;
            this.AccountID = statement.Budget.AccountID;
            this.AccountName = statement.Budget.Account.AccountName;
            this.CostCenterID = statement.Budget.CostCenterID;
            this.CostCenterName = statement.Budget.CostCenter.CostCenterName;
            this.BudgetAmount = statement.Budget.BudgetAmount;
        }
        public StatementViewModel(Statement statement, decimal previousAmount)
        {
            this.StatementID = statement.StatementID;
            this.PaymentID = statement.PaymentID;
            this.BudgetID = statement.BudgetID;
            this.Description = statement.Description;

            //Budget info
            this.BudgetYear = statement.Budget.Year;
            this.AccountID = statement.Budget.AccountID;
            this.AccountName = statement.Budget.Account.AccountName;
            this.CostCenterID = statement.Budget.CostCenterID;
            this.CostCenterName = statement.Budget.CostCenter.CostCenterName;
            this.BudgetAmount = statement.Budget.BudgetAmount;

            this.PreviousAmount = previousAmount;
            this.WithdrawAmount = statement.WithdrawAmount;
            this.RemainAmount = this.BudgetAmount - this.PreviousAmount - this.WithdrawAmount;
            this.Status = statement.Status.ToString();

            this.Payment = statement.Payment;
            this.Budget = statement.Budget;
        }


        public Guid StatementID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid BudgetID { get; set; }
        public string Description { get; set; }
        public decimal WithdrawAmount { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public string Status { get; set; }

        //Budget Info
        public string BudgetYear { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }
        public decimal BudgetAmount { get; set; }

        public Payment Payment { get; set; }
        public Budget Budget { get; set; }
    }
}