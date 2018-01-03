using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class PositionInfo
    {
        #region Constructor

        public PositionInfo()
        {

        }

        #endregion

        #region Fields

        public Guid Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(4)]
        public string PositionCode { get; set; }

        [StringLength(100)]
        public string PositionDescShort { get; set; }

        [StringLength(200)]
        public string PostionDesc { get; set; }

        #endregion


    }
}