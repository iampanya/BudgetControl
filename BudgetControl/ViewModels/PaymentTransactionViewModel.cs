using System;

namespace BudgetControl.ViewModels
{
    public class PaymentTransactionViewModel
    {
        #region Constructor

        public PaymentTransactionViewModel()
        {

        }

        #endregion

        #region Fields

        public Guid BudgetTransactionID { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string PaymentNo { get; set; }
        public string Description { get; set; }
        public string RequestBy { get; set; }
        public string RequestByFullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Amount { get; set; }

        #endregion

        #region Additional Fields

        public string CreatedAtText
        {
            get
            {
                return CreatedAt.ToString("dd MMM yyyy");
            }
        }

        #endregion

    }
}