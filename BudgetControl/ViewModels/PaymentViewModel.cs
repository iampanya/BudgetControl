using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class PaymentViewModel
    {
        #region Constructor

        public PaymentViewModel()
        {

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
            this.PaymentNo = payment.PaymentNo;
            this.Description = payment.Description;
            this.RequestBy = payment.RequestBy;
            this.PaymentDate = payment.PaymentDate;
            this.TotalAmount = payment.TotalAmount;
            this.Status = payment.Status;
        }

        #endregion

        #region Fields

        public Guid PaymentID { get; set; }

        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }

        public string Year { get; set; }

        public int Sequence { get; set; }

        public string PaymentNo { get; set; }

        public string Description { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string RequestBy { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? ContractorID { get; set; }
        public string ContractorName { get; set; }

        public RecordStatus Status { get; set; }
        public PaymentType Type { get; set; }

        public RecordTimeStamp RecordTimeStamp { get; set; }


        #endregion


        public string RequestByName
        {
            get
            {
                if(Type == PaymentType.Contractor)
                {
                    return ContractorName;
                }
                else
                {

                    return String.Join(" ", new String[] { FirstName, LastName });
                }
            }
        }

        public string StatusText
        {
            get
            {
                return Status.ToString();
            }
        }

        public string TypeText
        {
            get
            {
                return Type.ToString();
            }
        }

        public string PaymentDateText
        {
            get
            {
                return PaymentDate.ToString("d MMM yy", new CultureInfo("th-TH"));
            }
        }

    }


   

}