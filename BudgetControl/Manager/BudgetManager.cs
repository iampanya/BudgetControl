using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using BudgetControl.ViewModels;
using System.Configuration;
using System.Data.SqlClient;

namespace BudgetControl.Manager
{
    public class BudgetManager
    {
        private BudgetContext _db;
        private string conn_string = ConfigurationManager.ConnectionStrings["BudgetContext"].ConnectionString;

        private string cmd_budget_summary =
            @"
                SELECT b.BudgetID, b.AccountID, a.AccountName, b.CostCenterID
	                , b.BudgetAmount, ISNULL(d.Amount, 0) AS WithdrawAmount, (b.BudgetAmount - ISNULL(d.Amount, 0)) AS RemainAmount
	                , b.CreatedAt, b.CreatedBy, b.ModifiedAt, b.ModifiedBy
                FROM Budget b
		                LEFT OUTER JOIN (
			                SELECT	t.BudgetID, Sum(t.Amount) As Amount
			                FROM	BudgetTransaction t 
					                LEFT OUTER JOIN Payment p ON (t.PaymentID = p.PaymentID)
			                WHERE	t.Status = 1
					                AND p.Status = 1 
			                GROUP BY t.BUdgetID
		                ) d ON (b.BudgetID = d.BudgetID)
		                LEFT OUTER JOIN Account a ON (b.AccountID = a.AccountID)
                WHERE	b.CostCenterID like @CostCenterID
		                AND b.Year = @Year
		                AND b.Status = 1
                ORDER BY b.AccountID		
            ";
        #region Constructor

        public BudgetManager()
        {
            _db = new BudgetContext();
        }

        public BudgetManager(BudgetContext context)
        {
            this._db = context;
        }

        #endregion

        #region Properties

        public Budget Budget { get; set; }

        #endregion

        #region Methods

        public IQueryable<Budget> GetAll()
        {
            return _db.Budgets
                    .Include(b => b.Account)
                    .Include(b => b.CostCenter)
                    .OrderBy(b => b.AccountID)
                    .AsNoTracking();
        }

        public IQueryable<Budget> GetActive()
        {
            return GetAll()
                .Where(b => b.Status == BudgetStatus.Active);
        }

        public IEnumerable<Budget> GetByCostCenterID(string costcenterid)
        {
            return GetActive().Where(b => b.CostCenterID == costcenterid);
        }

        public Budget Get(Guid budgetid)
        {
            using (var budgetRepo = new BudgetRepository())
            {
                return budgetRepo.GetById(budgetid);
            }
        }

        public IEnumerable<Budget> GetWithSummary(string year, string costcenterid)
        {
            List<Budget> budgets = new List<Budget>();

            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(cmd_budget_summary, conn);
                cmd.Parameters.AddWithValue("CostCenterID", costcenterid);
                cmd.Parameters.AddWithValue("Year", year);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Budget budget = new Budget();
                    budget.BudgetID = Guid.Parse(reader["BudgetID"].ToString());
                    budget.AccountID = reader["AccountID"].ToString();
                    budget.Account = new Account();
                    budget.Account.AccountID = budget.AccountID;
                    budget.Account.AccountName = reader["AccountName"].ToString();
                    budget.CostCenterID = reader["CostCenterID"].ToString();
                    budget.BudgetAmount = decimal.Parse(reader["BudgetAmount"].ToString());
                    budget.WithdrawAmount = decimal.Parse(reader["WithdrawAmount"].ToString());
                    budget.RemainAmount = decimal.Parse(reader["RemainAmount"].ToString());
                    budget.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                    budget.CreatedBy = reader["CreatedBy"].ToString();
                    budget.ModifiedAt = DateTime.Parse(reader["ModifiedAt"].ToString());
                    budget.ModifiedBy = reader["ModifiedBy"].ToString();
                    budgets.Add(budget);
                }
            }
            return budgets;
        }

        public void Add(Budget budget)
        {
            // 1. Add Payment


            // 2. Add all transaction to payment
        }

        public void Update(Budget budget)
        {
            BudgetRepository budgetRepo = new BudgetRepository(_db);
            budget.NewModifyTimeStamp();
            budgetRepo.Update(budget);
            budgetRepo.Save();
        }

        public void Delete(Budget budget)
        {
           
        }


        #endregion

        #region Convert To VM

        public BudgetViewModel ConvertToVM(Budget budget)
        {
            return new BudgetViewModel(budget);
        }

        public IEnumerable<BudgetViewModel> ConvertToVMs(List<Budget> budgets)
        {
            List<BudgetViewModel> vms = new List<BudgetViewModel>();
            foreach(var budget in budgets)
            {
                vms.Add(ConvertToVM(budget));
            }
            return vms;
        }

        #endregion

    }
}