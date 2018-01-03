using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class PeaInfo : RecordTimeStamp
    {
        #region Constructor

        public PeaInfo()
        {

        }

        #endregion

        #region Field

        public Guid Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(6)]
        public string PeaCode { get; set; }

        [StringLength(100)]
        public string PeaShortName { get; set; }

        [StringLength(200)]
        public string PeaName { get; set; }

        #endregion
    }
}