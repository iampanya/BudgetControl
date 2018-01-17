using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public enum ConditionType
    {
        ExactMatch = 1,
        Between = 2,
        Contain = 3
    }

    public enum AuthorizeType
    {
        Employee = 1,
        CostCenter = 2
    }
    public class AuthorizeCostCenter : RecordTimeStamp
    {
        #region Constructor

        public AuthorizeCostCenter()
        {

        }

        #endregion

        #region Fields

        public Guid Id { get; set; }

        public AuthorizeType AuthorizeType { get; set; }

        [StringLength(8)]
        [Index()]
        public string EmployeeNo { get; set; }

        [StringLength(10)]
        [Index()]
        public string CostCenterCode { get; set; }
        
        public ConditionType Condition { get; set; }

        [StringLength(10)]
        public string CCAStart { get; set; }

        [StringLength(10)]
        public string CCAEnd { get; set; }

        public bool CanView { get; set; }

        public bool CanWithdraw { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
        
        [StringLength(32)]
        public string AuthorizeCode { get; set; }

        #endregion
    }
}