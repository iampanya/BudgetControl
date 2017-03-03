﻿using BudgetControl.Models.Base;
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

        public Budget(BudgetFileModel budgetfile)
        {
            this.BudgetID = Guid.NewGuid();
            this.AccountID = budgetfile.AccountID;
            this.CostCenterID = budgetfile.CostCenterID;
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
        public string BudgetName {
            get
            {
                return AccountID + " - " + Account.AccountName;
            }
        }

        #endregion
    }
}