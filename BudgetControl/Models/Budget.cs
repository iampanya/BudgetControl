using BudgetControl.DAL;
using BudgetControl.Manager;
using BudgetControl.Models.Base;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public enum BudgetStatus
    {
        [Display(Name = "ปกติ")]
        Active = 0x1,

        [Display(Name = "ปิด")]
        Closed = 0x2,

        [Display(Name = "ยกเลิก")]
        Cancelled = 0x3
    }

    public class Budget : RecordTimeStamp
    {
        #region Constructor

        public Budget()
        {

        }

        public Budget(Budget budget)
        {
            this.BudgetID = budget.BudgetID;
            this.AccountID = budget.AccountID;
            this.CostCenterID = budget.CostCenterID;
            this.Sequence = budget.Sequence;
            this.Year = budget.Year;
            this.BudgetAmount = budget.BudgetAmount;
            this.WithdrawAmount = budget.WithdrawAmount;
            this.RemainAmount = budget.RemainAmount;
            this.Status = budget.Status;
            this.Account = null;
            this.BudgetTransactions = null;

        }

        public Budget(BudgetFileModel budgetfile)
        {
            this.BudgetID = Guid.NewGuid();
            this.AccountID = budgetfile.AccountID;
            this.CostCenterID = budgetfile.CostCenterID;
            this.BudgetAmount = budgetfile.Amount;
            //this.RemainAmount = budgetfile.Amount;
            this.WithdrawAmount = 0;
            this.Year = budgetfile.Year;
            this.Status = BudgetStatus.Active;
        }

        #endregion

        #region Field

        public Guid BudgetID { get; set; }
        public string AccountID { get; set; }
        public string CostCenterID { get; set; }
        public int Sequence { get; set; }

        [Display(Name = "ปี")]
        [StringLength(4)]
        public string Year { get; set; }

        [Display(Name = "งบประมาณ")]
        public decimal BudgetAmount { get; set; }

        public decimal WithdrawAmount { get; set; }

        [Display(Name = "งบประมาณคงเหลือ")]
        public decimal RemainAmount { get; set; }

        public BudgetStatus Status { get; set; }

        #endregion

        #region Relation

        public virtual Account Account { get; set; }

        public virtual CostCenter CostCenter { get; set; }

        public virtual ICollection<Statement> Statements { get; set; }

        public virtual ICollection<BudgetTransaction> BudgetTransactions { get; set; }

        #endregion

        #region Additional field

        public string BudgetName
        {
            get
            {
                try
                {
                    return AccountID + " - " + Account.AccountName;
                }
                catch (Exception)
                {
                    return AccountID;
                }
            }
        }

        #endregion

        #region Validate

        public Base.ValidationResult Validate()
        {
            if (BudgetID == Guid.Empty)
            {
                return new Base.ValidationResult("BudgetID cannot be empty.", "BudgetID");
            }

            if (String.IsNullOrEmpty(AccountID))
            {
                return new Base.ValidationResult("AccountID cannot be empty.", "AccountID");
            }

            if (String.IsNullOrEmpty(CostCenterID))
            {
                return new Base.ValidationResult("CostCenterID cannot be empty.", "CostCenterID");
            }

            if (String.IsNullOrEmpty(Year))
            {
                return new Base.ValidationResult("Year cannot be empty.", "Year");
            }

            return new Base.ValidationResult(true);
        }

        #endregion

        #region Methods

        public void IncludeAccount()
        {
            if (String.IsNullOrEmpty(this.AccountID))
            {
                return;
            }
            using (var db = new BudgetContext())
            {
                AccountManager am = new AccountManager(db);
                this.Account = am.GetByID(this.AccountID);
            }
        }

        public void IncludeCostCenter()
        {
            if (String.IsNullOrEmpty(this.CostCenterID))
            {
                return;
            }
            using (var db = new BudgetContext())
            {
                this.CostCenter = db.CostCenters.Find(this.CostCenterID);
            }
        }

        public void IncludeTransactions()
        {
            using (var db = new BudgetContext())
            {
                TransactionManager tm = new TransactionManager(db);
                this.BudgetTransactions = tm.GetByBudget(this.BudgetID).ToList();
            }
        }

        #endregion
    }
}