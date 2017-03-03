using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class CostCenter : RecordTimeStamp
    {
        [Display(Name = "ศูนย์ต้นทุน")]
        [StringLength(10)]
        public string CostCenterID { get; set; }

        [Display(Name = "ศูนย์ต้นทุน")]
        [StringLength(128)]
        public string CostCenterName { get; set; }

        [Display(Name = "ศูนย์ต้นทุน")]
        [StringLength(64)]
        public string ShortName { get; set; }

        [Display(Name = "ศูนย์ต้นทุน")]
        [StringLength(128)]
        public string LongName {get; set; }

        public RecordStatus Status { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        public string CostCenterTrim
        {
            get
            {
                return CostCenterID == null ? string.Empty : CostCenterID.TrimEnd('0');
            }
        }

    }
}