using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class Contractor : RecordTimeStamp
    {
        #region Constructor

        public Contractor()
        {

        }

        #endregion

        #region Field

        public Guid ID { get; set; }

        public string CostCenterID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(100)]
        public string TitleName { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public RecordStatus Status { get; set; }

        #endregion

        #region Relationship

        public virtual CostCenter CostCenter { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        #endregion

    }
}