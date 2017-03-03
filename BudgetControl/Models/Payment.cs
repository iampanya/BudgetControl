using BudgetControl.Models.Base;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class Payment : RecordTimeStamp
    {

        #region Constructor
        public Payment()
        {
            
        }

        public Payment(Payment payment)
        {
            this.PaymentID = payment.PaymentID;
            this.CostCenterID = payment.CostCenterID;
            this.Year = payment.Year;
            this.Sequence = payment.Sequence;
            this.Description = payment.Description;
            this.RequestBy = payment.RequestBy;
            this.PaymentDate = payment.PaymentDate;
            this.TotalAmount = payment.TotalAmount;
            this.ControlBy = payment.ControlBy;
            this.Status = payment.Status;
            this.Requester = payment.Requester;
            this.Controller = payment.Controller;

            this.Statements = new List<Statement>();

            //TODO add all payment feilds

            //foreach (var statement in payment.Statements)
            //{
            //    this.Statements.Add(new Statement(statement));
            //}

        }

        public Payment(PaymentViewModel paymentviewmodel)
        {
            this.PaymentID = paymentviewmodel.PaymentID;
            this.CostCenterID = paymentviewmodel.CostCenterID;
            this.Year = paymentviewmodel.Year;
            this.Sequence = paymentviewmodel.Sequence;
            this.Description = paymentviewmodel.Description;
            this.RequestBy = paymentviewmodel.RequestBy;
            this.PaymentDate = paymentviewmodel.PaymentDate;
            this.TotalAmount = paymentviewmodel.TotalAmount;
            this.ControlBy = paymentviewmodel.ControlBy;

            this.Statements = new List<Statement>();
            if (paymentviewmodel.Statements != null)
            {
                paymentviewmodel.Statements.ForEach(
                    s => this.Statements.Add(new Statement(s)));
            }
        }


        #endregion

        #region Fields

        public Guid PaymentID { get; set; }
        public string CostCenterID { get; set; }

        [Display(Name = "ปี")]
        [StringLength(4)]
        public string Year { get; set; }

        [Display(Name = "ลำดับ")]
        public int Sequence { get; set; }

        [Display(Name = "รายละเอียด")]
        [StringLength(255)]
        public string Description { get; set; }

        [Display(Name = "เบิกโดย")]
        public string RequestBy { get; set; }

        [Display(Name = "เบิกวันที่")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "เบิกจำนวน")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "ผู้ควบคุมงบ")]
        public string ControlBy { get; set; }

        public RecordStatus Status { get; set; }

        #endregion

        #region Relation

        [ForeignKey("RequestBy")]
        public virtual Employee Requester { get; set; }

        [ForeignKey("ControlBy")]
        public virtual Employee Controller { get; set; }

        public virtual CostCenter CostCenter { get; set; }

        public virtual ICollection<Statement> Statements { get; set; }

        public virtual ICollection<BudgetTransaction> BudgetTransactions { get; set; }

        #endregion

        #region Additional Methods

        //public void InitialPayment(PaymentViewModel paymentviewmodel)
        //{
        //    this.PaymentID = paymentviewmodel.PaymentID;
        //    this.CostCenterID = paymentviewmodel.CostCenterID;
        //    this.Year = paymentviewmodel.Year;
        //    this.Sequence = paymentviewmodel.Sequence;
        //    this.Description = paymentviewmodel.Description;
        //    this.RequestBy = paymentviewmodel.RequestBy;
        //    this.PaymentDate = paymentviewmodel.PaymentDate;
        //    this.TotalAmount = paymentviewmodel.TotalAmount;
        //    this.ControlBy = paymentviewmodel.ControlBy;
            
        //    RecordStatus rs;
        //    Enum.TryParse(paymentviewmodel.Status, out rs);
        //    this.Status = rs;

        //}
        #endregion

    }
}