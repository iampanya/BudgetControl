using BudgetControl.Models.Base;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class Account : RecordTimeStamp
    {
        #region Constructor
        public Account()
        {

        }

        public Account(BudgetFileModel budgetfile)
        {
            this.AccountID = budgetfile.AccountID;
            this.AccountName = budgetfile.AccountName;
            this.Status = RecordStatus.Active;
        }

        #endregion

        #region Fields

        [Display(Name = "รหัสบัญชี")]
        [StringLength(10)]
        public string AccountID { get; set; }

        [Display(Name = "ชื่อบัญชี")]
        [StringLength(128)]
        public string AccountName { get; set; }

        public RecordStatus Status { get; set; }

        #endregion


        #region Relationship

        public virtual ICollection<Budget> Budgets { get; set; }

        #endregion


    }
}