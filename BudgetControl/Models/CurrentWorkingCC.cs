using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class CurrentWorkingCC : RecordTimeStamp
    {
        #region Constructor

        public CurrentWorkingCC()
        {

        }

        #endregion

        #region Fields

        public Guid Id { get; set; }

        [StringLength(10)]
        public string EmployeeID { get; set; }

        [StringLength(10)]
        public string WorkingCostCenterID { get; set; }



        #endregion
    }

}