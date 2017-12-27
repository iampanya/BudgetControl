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

        #region SQL Command

        #region CMD: Budget Summary

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

        #endregion

        #region CMD: Budget Info

        private string cmd_budget_info =
            @"
                SELECT TOP(1) b.BudgetID, b.AccountID, a.AccountName, b.CostCenterID, c.CostCenterName, b.Year, b.BudgetAmount, b.Status
                FROM	Budget b
		                INNER JOIN Account a ON (b.AccountID = a.AccountID)
		                INNER JOIN CostCenter c ON (b.CostCenterID = c.CostCenterID) 
                WHERE	BudgetID = @BudgetID
		                AND b.Status = 1
            ";

        #endregion

        #region CMD: Budget with transaction

        private string cmd_budget_transaction =
            @"
                SELECT  bt1.BudgetTransactionID, p.Description, bt1.Amount, bt2.PreviousAmount
		                , bt1.CreatedAt, bt1.CreatedBy, bt1.ModifiedAt, bt1.ModifiedBy
		                , p.PaymentID, p.PaymentNo, p.Type, p.RequestBy, p.ContractorID
                FROM	BudgetTransaction bt1
		                INNER JOIN Payment p ON (bt1.PaymentID = p.PaymentID)
		                INNER JOIN (
			                SELECT	t1.BudgetTransactionID, (SUM(t2.Amount) - t1.Amount) As PreviousAmount
			                FROM	BudgetTransaction t1 
					                LEFT OUTER JOIN BudgetTransaction t2 ON (t1.CreatedAt >= t2.CreatedAt)
					                INNER JOIN Payment p ON (t1.PaymentID = p.PaymentID)
			                WHERE	t1.BudgetID = @BudgetID 
					                AND t2.BudgetID = @BudgetID
					                AND t2.Status = 1
					                AND p.Status = 1
			                GROUP BY t1.BudgetTransactionID, t1.Amount
		                ) AS bt2 ON (bt1.BudgetTransactionID = bt2.BudgetTransactionID)
                WHERE bt1.BudgetID = @BudgetID
		                AND bt1.Status = 1
		                AND p.Status = 1
                ORDER BY bt1.CreatedAt
            ";

        #endregion

        #endregion
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

        #region Common Get

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

        public Budget GetByID(Guid budgetid)
        {
            using (var budgetRepo = new BudgetRepository())
            {
                return budgetRepo.GetById(budgetid);
            }
        }

        #endregion

        #region Get budget with transaction

        public Budget GetWithTransaction(string id)
        {
            Budget budget;

            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(cmd_budget_info, conn))
                {
                    cmd.Parameters.AddWithValue("BudgetID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            budget = new Budget();
                            budget.BudgetID = Guid.Parse(id);
                            budget.AccountID = reader["AccountID"].ToString();
                            budget.Account = new Account()
                            {
                                AccountID = budget.AccountID,
                                AccountName = reader["AccountName"].ToString()
                            };
                            budget.CostCenterID = reader["CostCenterID"].ToString();
                            budget.CostCenter = new CostCenter()
                            {
                                CostCenterID = budget.CostCenterID,
                                CostCenterName = reader["CostCenterName"].ToString()
                            };
                            budget.Year = reader["Year"].ToString();
                            budget.BudgetAmount = decimal.Parse(reader["BudgetAmount"].ToString());
                            budget.Status = (BudgetStatus)int.Parse(reader["Status"].ToString());
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand(cmd_budget_transaction, conn))
                {
                    cmd.Parameters.AddWithValue("BudgetID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        budget.BudgetTransactions = new List<BudgetTransaction>();
                        budget.WithdrawAmount = 0;
                        while (reader.Read())
                        {
                            BudgetTransaction bt = new BudgetTransaction();
                            bt.BudgetTransactionID = Guid.Parse(reader["BudgetTransactionID"].ToString());
                            bt.Description = reader["Description"].ToString();
                            bt.Amount = decimal.Parse(reader["Amount"].ToString());
                            bt.PreviousAmount = decimal.Parse(reader["PreviousAmount"].ToString());
                            bt.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                            bt.CreatedBy = reader["CreatedBy"].ToString();
                            bt.ModifiedAt = DateTime.Parse(reader["ModifiedAt"].ToString());
                            bt.ModifiedBy = reader["ModifiedBy"].ToString();
                            bt.PaymentID = Guid.Parse(reader["PaymentID"].ToString());
                            bt.Payment = new Payment()
                            {
                                PaymentNo = reader["PaymentNo"].ToString(),
                                Type = (PaymentType)int.Parse(reader["Type"].ToString()),
                                RequestBy = reader["RequestBy"].ToString(),
                                Description = reader["Description"].ToString(),
                                ContractorID = reader.IsDBNull(reader.GetOrdinal("ContractorID")) ? (Guid?)null : Guid.Parse(reader["ContractorID"].ToString())
                            };
                            bt.RemainAmount = budget.BudgetAmount - bt.Amount - bt.PreviousAmount;
                            budget.BudgetTransactions.Add(bt);
                            budget.WithdrawAmount += bt.Amount;
                        }

                    }
                }
                conn.Close();
            }

            budget.RemainAmount = budget.BudgetAmount - budget.WithdrawAmount;

            return budget;
        }

        #endregion

        #region Get Overall Budget by CostCenter and Year

        public IEnumerable<Budget> GetOverall(string year, string costcenterid)
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

        #endregion

        #region Create

        public void Add(Budget budget)
        {
            // 1. Add Payment


            // 2. Add all transaction to payment
        }

        #endregion

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
            foreach (var budget in budgets)
            {
                vms.Add(ConvertToVM(budget));
            }
            return vms;
        }

        #endregion

    }
}