using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.Models.Temp
{
    public class TempBudget
    {
        #region Constructor

        public TempBudget()
        {

        }

        #endregion

        #region Fields

        public Guid Id { get; set; }

        [StringLength(8)]
        public string AccountID { get; set; }

        [StringLength(255)]
        public string AccountName { get; set; }

        [StringLength(10)]
        public string CostCenterID { get; set; }

        [StringLength(4)]
        public string Year { get; set; }

        public decimal BudgetAmount { get; set; }

        [StringLength(50)]
        public string UploadBy { get; set; }

        public DateTime? UploadTime { get; set; }

        #endregion
    }
}