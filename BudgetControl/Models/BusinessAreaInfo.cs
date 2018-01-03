using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class BusinessAreaInfo
    {
        #region Constructor

        public BusinessAreaInfo()
        {

        }

        #endregion

        #region Field

        public Guid Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(4)]
        public string BaCode { get; set; }

        [StringLength(200)]
        public string BaName { get; set; }

        #endregion
    }
}