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

    #region BudgetIndexViewModel

    public class BudgetViewModel
    {
        #region Constructor


        public BudgetViewModel()
        {

        }
        public BudgetViewModel(Budget budget)
        {
            BudgetID = budget.BudgetID;
            AccountID = budget.AccountID;
            CostCenterID = budget.CostCenterID;
            Year = budget.Year;
            BudgetAmount = budget.BudgetAmount;
            Status = budget.Status;
            AccountName = budget.Account != null? budget.Account.AccountName: "-";
            CostCenterName = budget.CostCenter != null ? budget.CostCenter.CostCenterName: "-";

            ////Calculate 
            //WithdrawAmount = 0;
            //using (StatementRepository statementRepo = new StatementRepository())
            //{
            //    var statements = statementRepo.GetByBudget(budget.BudgetID).ToList();
            //    statements.ForEach(s => WithdrawAmount = WithdrawAmount + s.WithdrawAmount);
            //    RemainAmount = BudgetAmount - WithdrawAmount;
            //}
        }

        #endregion

        #region Fields

        public Guid BudgetID { get; set; }
        public string AccountID { get; set; }
        public string CostCenterID { get; set; }
        public string Year { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal WithdrawAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public BudgetStatus Status { get; set; }
        public string AccountName { get; set; }
        public string CostCenterName { get; set; }
        public List<StatementViewModel> Statements { get; set; }

        #endregion

        #region Get Properties
        public string BudgetName
        {
            get
            {
                try
                {
                    return AccountID + " - " + AccountName;
                }
                catch (Exception ex)
                {
                    return AccountID;
                }
            }
        }

        public string Description 
        {
            get
            {
                return "[" + this.CostCenterID + "] " + this.AccountID + " - " + this.AccountName;
            }
        }

        #endregion

        #region Additional Method
        public void GetDetails()
        {
            this.Statements = new List<StatementViewModel>();
            using (StatementRepository statementRep = new StatementRepository())
            {
                decimal previousamount = 0;
                statementRep
                    .GetByBudget(this.BudgetID)
                    .ToList()
                    .ForEach(s => {
                        this.Statements.Add(new StatementViewModel(s, previousamount));
                        previousamount += s.WithdrawAmount;
                    });
            }
        }

        #endregion

    }

    #endregion


    public class UploadBudgetModel
    {
        #region Constructor

        public UploadBudgetModel()
        {

        }

        #endregion

        #region Fields

        public string Year { get; set; }
        public string CostCenterBegin { get; set; }
        public string CostCenterEnd { get; set; }
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

}