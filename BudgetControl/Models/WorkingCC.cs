using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class WorkingCC : RecordTimeStamp
    {
        #region Constructor 

        public WorkingCC ()
        {

        }

        #endregion

        #region Fields

        public Guid Id { get; set; }

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

        #endregion

        #region Methods

        public bool IsValid()
        {
            if(String.IsNullOrEmpty(EmployeeNo) && String.IsNullOrEmpty(CostCenterCode))
            {
                return false;
            }

            if (String.IsNullOrEmpty(CCAStart))
            {
                return false;
            }

            if(Condition == ConditionType.Between && String.IsNullOrEmpty(CCAEnd))
            {
                return false;
            }
            
            if(!Enum.IsDefined(typeof(ConditionType), Condition))
            {
                return false;
            }

            return true;
        }

        #endregion


    }
}