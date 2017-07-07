using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;

namespace BudgetControl.ViewModels
{
    #region Budget Index

    /**
     * Budget viewmodel for budget index page.
    **/
    public class BudgetIndexViewModel
    {

        #region Constructor

        public BudgetIndexViewModel()
        {
            InitInstance();
        }

        public BudgetIndexViewModel(Budget budget)
        {
            // 1. Initial instance
            InitInstance();

            // 2. Check parameters
            if(budget == null)
            {
                throw new ArgumentNullException();
            }
            
            // 3. Fill data
            //// 3.1 Budget data
            BudgetID = budget.BudgetID;
            Year = budget.Year;
            BudgetAmount = budget.BudgetAmount;
            Status = budget.Status.ToString();
            WithdrawAmount = budget.WithdrawAmount;
            RemainAmount = budget.RemainAmount;

            //// 3.2 Account data
            AccountID = budget.AccountID;
            if(budget.Account != null)
            {
                AccountName = budget.Account.AccountName;
            }
            else
            {
                AccountName = GetAccountName(AccountID);
            }
            
            //// 3.3 CostCenter data
            CostCenterID = budget.CostCenterID;
            if(budget.CostCenter != null)
            {
                CostCenterName = budget.CostCenter.CostCenterName;
            }
            else
            {
                CostCenterName = GetCostCenterName(CostCenterID);
            }


            //// 3.5 Timestamp
            Timestamp.CreatedAt = budget.CreatedAt;
            Timestamp.CreatedBy = budget.CreatedBy;
            Timestamp.ModifiedAt = budget.ModifiedAt;
            Timestamp.ModifiedBy = budget.ModifiedBy;
        }

        #endregion

        #region Fields

        public Guid BudgetID { get; set; }

        public string Year { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal WithdrawAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public string Status { get; set; }

        // Account 
        public string AccountID { get; set; }
        public string AccountName { get; set; }

        // CostCenter 
        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }

        public RecordTimeStamp Timestamp { get; set; }

        #endregion

        #region Methods

        /**
         *  Initial instance
        **/
        private void InitInstance()
        {
            Timestamp = new RecordTimeStamp();
        }

        /**
         * 
        **/
        private string GetAccountName(string accountid)
        {
            BudgetContext db = new BudgetContext();
            Account account = db.Accounts.Find(accountid);
            return account == null ? "" : account.AccountName;
        }

        /**
         * 
        **/
        private string GetCostCenterName(string costcenterid)
        {
            BudgetContext db = new BudgetContext();
            CostCenter costcenter = db.CostCenters.Find(costcenterid);
            return costcenter == null ? "" : costcenter.CostCenterName;
        }


        #endregion

    }

    #endregion

    #region Budget Details

    /**
     * Detail of budget for budget detail page.
    **/
    public class BudgetDetailViewModel : BudgetIndexViewModel
    {
        #region Constructor

        public BudgetDetailViewModel() : base()
        {
            InitInstance();
        }

        public BudgetDetailViewModel(Budget budget) : base(budget)
        {
            InitInstance();

            if (budget.BudgetTransactions != null)
            {
                foreach (var item in budget.BudgetTransactions)
                {
                    Transactions.Add(new TransactionIndexViewModel(item));
                }
            }

        }

        #endregion

        #region Fields

        public List<TransactionIndexViewModel> Transactions { get; set; }

        #endregion

        #region Methods

        private void InitInstance()
        {
            Transactions = new List<TransactionIndexViewModel>();
        }

        #endregion

    }

    #endregion

    #region Upload budget form
    
    public class UploadBudgetModel
    {
        #region Constructor

        public UploadBudgetModel()
        {

        }

        #endregion

        #region Fields

        public string Year { get; set; }
        public string FileData { get; set; }

        #endregion

        #region Methods

        public void Validate()
        {
            if (String.IsNullOrEmpty(this.Year))
            {
                throw new Exception("กรุณาป้อนข้อมูล ปีงบประมาณ");
            }

            if (String.IsNullOrEmpty(this.Year))
            {
                throw new Exception("กรุณาเลือกไฟล์ที่ต้องการ");
            }
        }

        #endregion
    }

    #endregion

    #region Budget file structure

    public class BudgetFileModel
    {
        #region Constructor

        public BudgetFileModel() 
        { 

        }

        public BudgetFileModel(string[] columns, string year)
        {
            this.AccountID = columns[1].Trim();
            this.AccountName = columns[2].Trim();
            this.Amount = ConvertToDecimal(columns[3]);
            this.CostCenterID = columns[4].Trim();
            this.Year = year;
        }

        #endregion

        #region Fields

        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string CostCenterID { get; set; }
        public string Year { get; set; }

        #endregion

        #region Methods

        public decimal ConvertToDecimal(string amount)
        {
            var s_amount = amount.Trim();
            var split = s_amount.Split('-');
            var value = decimal.Parse(split[0]);
            if (split.Length > 1)
            {
                return 0 - value;
            }
            else
            {
                return value;
            }
        }

        #endregion
    }

    #endregion

    #region Create budget form

    public class CreateBudgetModel
    {
        #region Constructor

        public CreateBudgetModel()
        {

        }

        #endregion

        #region Fields

        public string Year { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string CostCenterID { get; set; }
        public decimal Amount { get; set; }

        #endregion
    }

    #endregion

}