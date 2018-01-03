using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class LevelInfo : RecordTimeStamp
    {
        #region Constructor

        public LevelInfo()
        {

        }

        #endregion

        #region Field

        public Guid Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(2)]
        public string LevelCode { get; set; }

        [StringLength(100)]
        public string LevelDesc { get; set; }

        #endregion
    }
}