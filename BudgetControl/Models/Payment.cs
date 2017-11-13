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
    public enum PaymentType
    {
        Normal = 1,
        ZCCA = 2,
        Allowance = 3
    }

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
            this.PaymentNo = payment.PaymentNo;
            this.Description = payment.Description;
            this.RequestBy = payment.RequestBy;
            this.PaymentDate = payment.PaymentDate;
            this.TotalAmount = payment.TotalAmount;
            this.ControlBy = payment.ControlBy;
            this.Status = payment.Status;

        }

        public Payment(PaymentViewModel paymentviewmodel)
        {
            this.PaymentID = paymentviewmodel.PaymentID;
            this.CostCenterID = paymentviewmodel.CostCenterID;
            this.Year = paymentviewmodel.Year;
            this.Sequence = paymentviewmodel.Sequence;
            this.PaymentNo = paymentviewmodel.PaymentNo;
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

        [StringLength(4)]
        public string Year { get; set; }

        [StringLength(35)]
        public string PaymentNo { get; set; }

        public int Sequence { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string RequestBy { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string ControlBy { get; set; }

        public RecordStatus Status { get; set; }


        public string OwnerCostCenterID { get; set; }

        public PaymentType Type { get; set; }

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

        #region Validate

        public void Validate()
        {
            
        }

        #endregion

        #region Additional Methods
        
        public void PrepareToSave()
        {
            this.PaymentID = Guid.NewGuid();
            this.PaymentDate = DateTime.Now;
            this.NewCreateTimeStamp();
            this.Status = RecordStatus.Active;
        }


        #endregion

    }
}