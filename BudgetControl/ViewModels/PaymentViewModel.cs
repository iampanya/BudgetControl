using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class PaymentViewModel
    {
        #region Constructor

        public PaymentViewModel()
        {
            this.Statements = new List<StatementViewModel>();
        }

        public PaymentViewModel(Payment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException();
            }

            this.PaymentID = payment.PaymentID;
            this.CostCenterID = payment.CostCenterID;
            this.CostCenterName = payment.CostCenter.CostCenterName;
            this.Year = payment.Year;
            this.Sequence = payment.Sequence;
            this.Description = payment.Description;
            this.RequestBy = payment.RequestBy;
            this.PaymentDate = payment.PaymentDate;
            this.TotalAmount = payment.TotalAmount;
            this.ControlBy = payment.ControlBy;
            this.Status = payment.Status.ToString();
            this.Controller = payment.Controller;
            this.Requester = payment.Requester;
        }

        #endregion

        #region Fields

        public Guid PaymentID { get; set; }
        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }
        public string Year { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public string RequestBy { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ControlBy { get; set; }
        public string Status { get; set; }

        public Employee Requester { get; set; }
        public Employee Controller { get; set; }

        public List<StatementViewModel> Statements { get; set; }

        #endregion

        #region Additional Method
        public void GetDetails()
        {
            this.Statements = new List<StatementViewModel>();
            using (StatementRepository statementRep = new StatementRepository())
            {
                statementRep
                    .GetByPayment(this.PaymentID)
                    .ToList()
                    .ForEach(s => this.Statements.Add(new StatementViewModel(s)));
            }
        }

        #endregion
    }


   

}