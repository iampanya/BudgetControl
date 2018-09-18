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
        Internal = 1, 
        PEA = 2,
        Contractor = 3
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

            this.Type = payment.Type;
        }

        public Payment(PaymentViewModel vm)
        {
            this.PaymentID = vm.PaymentID;
            this.CostCenterID = vm.CostCenterID;
            this.Year = vm.Year;
            this.PaymentNo = vm.PaymentNo;
            this.Sequence = vm.Sequence;
            this.Description = vm.Description;
            this.RequestBy = vm.RequestBy;
            this.PaymentDate = vm.PaymentDate;
            this.TotalAmount = vm.TotalAmount;
            this.Status = vm.Status;
            this.Type = vm.Type;
            this.ContractorID = vm.ContractorID;
            
            if(vm.Type == PaymentType.Contractor && vm.ContractorID != null)
            {
                Contractor = new Contractor();
                Contractor.ID = (Guid)vm.ContractorID;
                Contractor.Name = vm.ContractorName;
            }

            if(vm.Transactions != null)
            {
                this.BudgetTransactions = new List<BudgetTransaction>();
                foreach(var item in vm.Transactions)
                {
                    var trans = new BudgetTransaction(item);
                    BudgetTransactions.Add(trans);
                }
            }
        }


        #endregion

        #region Fields

        public Guid PaymentID { get; set; }

        public string CostCenterID { get; set; }
        
        [StringLength(4)]
        [Index()]
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

        public PaymentType Type { get; set; }

        public Guid? ContractorID { get; set; }

        #endregion

        #region Relation

        [ForeignKey("RequestBy")]
        public virtual Employee Requester { get; set; }

        [ForeignKey("ContractorID")]
        public virtual Contractor Contractor { get; set; }

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


        #region Additional Fields

        public string RequestByName
        {
            get
            {
                string name = string.Empty;
                switch (this.Type)
                {
                    case PaymentType.Internal:
                        return this.Requester != null ? this.Requester.FullName : "-";
                    case PaymentType.PEA:
                        return this.Requester != null ? this.Requester.FullName : "-";
                    case PaymentType.Contractor:
                        return this.Contractor != null ? this.Contractor.Name: "-";
                    default:
                        return "";
                }
            }
        }

        #endregion
    }
}