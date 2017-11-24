using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class BudgetExportViewModel
    {
        #region Constructor

        public BudgetExportViewModel()
        {

        }

        public BudgetExportViewModel(BudgetTransaction trans)
        {
            Date = trans.Payment.PaymentDate.Date.ToString("dd/MM/yyyy");
            PaymentNo = trans.Payment.PaymentNo;
            Description = trans.Payment.Description;
            Amount = trans.Amount;
            PreviousAmount = trans.PreviousAmount;
            RemainAmount = trans.RemainAmount;
            RequestBy = trans.Payment.RequestBy;
            Controller = trans.Payment.CreatedBy;
        }

        #endregion

        [Display( Name = "วันที่เบิก")]
        public string Date { get; set; }


        [Display(Name ="เลขที่ใบสำคัญจ่าย")]
        public string PaymentNo { get; set; }
        
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }

        [Display(Name = "จำนวนเงินที่เบิก")]
        public decimal Amount { get; set; }

        [Display(Name = "ยอดก่อนหน้า")]
        public decimal PreviousAmount { get; set; }

        [Display(Name = "คงเหลือ")]
        public decimal RemainAmount { get; set; }

        [Display(Name = "ผู้เบิก")]
        public string RequestBy { get; set; }

        [Display(Name = "ผู้ตัดงบ")]
        public string Controller { get; set; }
    }
}