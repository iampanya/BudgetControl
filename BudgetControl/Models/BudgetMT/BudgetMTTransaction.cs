using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BudgetControl.Models.BudgetMT
{
    public class BudgetMTTransaction
    {
        public Guid Id { get; set; }
        public Guid BudgetId { get; set; }
        public string Year { get; set; }
        public string MTNo { get; set; }
        public string Title { get; set; }
        public string OwnerDepartment { get; set; }
        public string OwnerCostCenter { get; set; }
        public string Participant { get; set; }
        public DateTime? SeminarDate { get; set; }
        public string Location { get; set; }
        public int ParticipantCount { get; set; }
        public int MealCount { get; set; }
        public bool HasMealMorning { get; set; }
        public bool HasMealAfternoon { get; set; }
        public string Remark { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string BudgetCostCenter { get; set; }
        public string AccountID { get; set; }

        #region Additional field

        public string SeminarDateText
        {
            get
            {
                return SeminarDate?.ToString("d MMM yy", new CultureInfo("th-TH"));
            }
        }

        public string InputSeminarDate { get; set; } // format = dd/MM/yyyy 

        public string TotalAmountText
        {
            get
            {
                return TotalAmount.ToString("#,##0.00");
            }
        }
        public string RemainAmountText
        {
            get
            {
                return RemainAmount.ToString("#,##0.00");
            }
        }
        #endregion
    }
}