using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class PaymentCounter : RecordTimeStamp
    {
        #region Constructor

        public PaymentCounter()
        {

        }

        #endregion

        #region Fields

        public int PaymentCounterID { get; set; }

        [ForeignKey("CostCenter")]
        public string CostCenterID { get; set; }

        [StringLength(4)]
        [Index()]
        public string Year { get; set; }    
        
        [StringLength(30)]
        public string ShortCode { get; set; }

        public int Number { get; set; } 
        

        #endregion

        #region Relation
        
        public virtual CostCenter CostCenter { get; set; }

        #endregion

    }
}