using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class ReportManager
    {
        string conn_string = ConfigurationManager.ConnectionStrings["BudgetContext"].ConnectionString;

        string cmd_summary_report = @"
---- Summary Report
--DECLARE @Year varchar(4)
--SET @Year = '2561'

--DECLARE @DeptSap int
--SET @DeptSap = 3180 -- ฝบพ
--SET @DeptSap = 7731 -- ผสบ
--SET @DeptSap = 7730 -- กรท
--SET @DeptSap = 3108 -- แกลง

-- Get CostCenter Belong To
DECLARE  @CostCenter TABLE(CostCenterID  varchar(10))
;WITH DeptTree as(
    SELECT * FROM DepartmentInfo
    WHERE DeptSap = @DeptSap

    UNION ALL

    SELECT d.* FROM DepartmentInfo d
    INNER JOIN DeptTree x ON d.DeptUpper= x.DeptSap
) 

INSERT INTO @CostCenter (CostCenterID)
(
	SELECT CostCenterID 
	FROM CostCenter 
	WHERE CostCenterID IN 
		(
			SELECT CostCenterCode FROM DeptTree 
		)
		AND [Status] = 1
)

-- Get Summary Report
SELECT	b.AccountID
		, a.AccountName
		, MAX(total_budget.BudgetAmount) As BudgetAmount
		, ISNULL(MAX(total_payment.WithdrawAmount), 0) AS WithdrawAmount
		, ISNULL(ISNULL(MAX(total_budget.BudgetAmount),0) - ISNULL(MAX(total_payment.WithdrawAmount), 0), 0) AS RemainAmount
FROM Budget b
	LEFT OUTER JOIN (
		SELECT b.AccountID, SUM(b.BudgetAmount) As BudgetAmount
		FROM budget b
		WHERE b.CostCenterID IN (SELECT * FROM @CostCenter)
				AND b.Year = @Year
				AND b.Status = 1
		GROUP BY b.AccountID
	) AS total_budget ON (b.AccountID = total_budget.AccountID)
	LEFT OUTER JOIN (
		SELECT b.AccountID, ISNULL(SUM(t.Amount), 0) AS WithdrawAmount
			FROM payment p 
				INNER JOIN BudgetTransaction t on (p.PaymentID = t.PaymentID)
				INNER JOIN Budget b on (t.BudgetID = b.BudgetID)
			WHERE b.CostCenterID IN (SELECT * FROM @CostCenter)
					AND p.year = @Year
					AND p.status = 1 and t.Status = 1
			GROUP BY b.AccountID
	) AS total_payment ON (b.AccountID = total_payment.AccountID)
	LEFT OUTER JOIN Account a ON (b.AccountID = a.AccountID)
WHERE b.CostCenterID IN (SELECT * FROM @CostCenter)
		AND b.Year = @Year
		AND b.Status = 1
GROUP BY b.AccountID, a.AccountName
ORDER BY b.AccountID
        ";

        string cmd_individual_report = @"
            SELECT b.BudgetID, b.AccountID, a.AccountName, b.CostCenterID, b.BudgetAmount, ISNULL(total_payment.TotalWithdrawAmount, 0) AS TotalWithDrawAmount
            FROM Budget b
		            LEFT OUTER JOIN (
			            SELECT t.BudgetID, Sum(t.Amount) AS TotalWithdrawAmount
			            FROM BudgetTransaction t
					            INNER JOIN Payment p ON (t.PaymentID = p.PaymentID)
			            WHERE	p.RequestBy = @EmployeeID
					            AND p.Status = 1 
					            AND t.Status = 1
			            GROUP BY t.BudgetID
		            ) AS total_payment ON (b.BudgetID = total_payment.BudgetID)
		            LEFT OUTER JOIN Account a ON (b.AccountID = a.AccountID)
            WHERE	b.CostCenterID = @CostCenterID
		            AND b.Year = @Year
		            AND b.Status = 1
            ORDER BY b.AccountID
        ";

        #region Constructor

        public ReportManager()
        {

        }

        #endregion

        #region Summary Report

        public IEnumerable<Budget> SummaryReport(int deptSap, string year)
        {
            List<Budget> budgets = new List<Budget>();
            if (String.IsNullOrEmpty(year))
            {
                year = (DateTime.Today.Year + 543).ToString();
            }

            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(cmd_summary_report, conn);
                cmd.Parameters.AddWithValue("DeptSap", deptSap);
                cmd.Parameters.AddWithValue("Year", year);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Budget budget = new Budget();
                    budget.AccountID = reader["AccountID"].ToString();
                    budget.Account = new Account();
                    budget.Account.AccountName = reader["AccountName"].ToString();
                    budget.BudgetAmount = decimal.Parse(reader["BudgetAmount"].ToString());
                    budget.WithdrawAmount = decimal.Parse(reader["WithdrawAmount"].ToString());
                    budget.RemainAmount = decimal.Parse(reader["RemainAmount"].ToString());
                    budgets.Add(budget);
                }

            }

            return budgets;
        }


        public IEnumerable<Budget> SummaryReport_EF(CostCenter costcenter, string year)
        {
            List<Budget> budgets;

            // 1. Get budget data.
            using (BudgetRepository budgetRep = new BudgetRepository())
            {
                budgets = budgetRep.Get().ToList();
                budgets = budgetRep.Get()
                    .Where(
                        b =>
                            b.CostCenterID == costcenter.CostCenterID &&
                            b.Year == year &&
                            b.Status == BudgetStatus.Active

                    )
                    .ToList();
            }

            // 2. Get budget details
            foreach (var budget in budgets)
            {
                // Get budget inside costcenter
                List<Guid> budgetincharge = new List<Guid>();
                using (var budgetRepo = new BudgetRepository())
                {
                    List<Budget> listbudget = budgetRepo.Get()
                        .Where(b =>
                            b.AccountID == budget.AccountID &&
                            b.CostCenterID.StartsWith(costcenter.CostCenterTrim) &&
                            b.Status == BudgetStatus.Active).ToList();

                    budgetincharge.Clear();
                    if (listbudget != null)
                    {
                        listbudget.ForEach(b =>
                        {
                            // Add to budgetincharge list
                            budgetincharge.Add(b.BudgetID);

                            // Summary budget amount
                            if (b.CostCenterID != costcenter.CostCenterID)
                            {
                                var index = budgets.FindIndex(a => a.AccountID == b.AccountID);
                                if (index >= 0)
                                {
                                    budgets[index].BudgetAmount += b.BudgetAmount;
                                }
                            }
                        });
                    }
                }


                // Get all transaction inside budget
                List<BudgetTransaction> transactions;
                using (var tranRepo = new TransactionRepository())
                {
                    transactions = tranRepo.Get().Where(
                        t =>
                            budgetincharge.Contains(t.BudgetID) &&
                            t.Status == RecordStatus.Active &&
                            t.Budget.Year == year

                        ).ToList();
                }
                decimal wdAmount = 0;
                transactions.ForEach(t => wdAmount += t.Amount);

                budget.WithdrawAmount = wdAmount;
                budget.RemainAmount = budget.BudgetAmount - budget.WithdrawAmount;
            }

            // 3. Set return object.
            return budgets;
        }

        #endregion


        #region Individual Report

        public IEnumerable<Budget> Individual(Employee employee, string year)
        {
            List<Budget> budgets = new List<Budget>();
            if (String.IsNullOrEmpty(year))
            {
                year = (DateTime.Today.Year + 543).ToString();
            }

            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(cmd_individual_report, conn))
                {
                    cmd.Parameters.AddWithValue("EmployeeID", employee.EmployeeID);
                    cmd.Parameters.AddWithValue("CostCenterID", employee.CostCenterID);
                    cmd.Parameters.AddWithValue("Year", year);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Budget budget = new Budget();
                            budget.BudgetID = Guid.Parse(reader["BudgetID"].ToString());
                            budget.AccountID = reader["AccountID"].ToString();
                            budget.Account = new Account();
                            budget.Account.AccountName = reader["AccountName"].ToString();
                            budget.BudgetAmount = decimal.Parse(reader["BudgetAmount"].ToString());
                            budget.CostCenterID = reader["CostCenterID"].ToString();
                            budget.WithdrawAmount = decimal.Parse(reader["TotalWithdrawAmount"].ToString());
                            budgets.Add(budget);
                        }
                    }
                }
            }

            return budgets;
        }

        public IEnumerable<Budget> Individual_EF(Employee employee, string year)
        {
            List<Budget> budgets;

            using (BudgetRepository budgetRep = new BudgetRepository())
            {
                budgets = budgetRep.Get().ToList();
                budgets = budgetRep.Get()
                    .Where(
                        b =>
                            b.CostCenterID == employee.CostCenterID &&
                            b.Year == year
                    )
                    .ToList();
            }

            foreach (var budget in budgets)
            {
                using (PaymentRepository paymentRepo = new PaymentRepository())
                {
                    //budget.BudgetTransactions = budget.BudgetTransactions.ToList().ForEach(t => t.Payment = paymentRepo.GetById(t.PaymentID));

                    foreach (var item in budget.BudgetTransactions)
                    {
                        item.Payment = paymentRepo.GetById(item.PaymentID);
                    }
                }
                List<BudgetTransaction> transactions = budget.BudgetTransactions
                    .Where(
                        t =>
                            t.Status == RecordStatus.Active &&
                            t.Payment.RequestBy == employee.EmployeeID)
                    .ToList();

                decimal wdAmount = 0;
                transactions.ForEach(t => wdAmount += t.Amount);

                budget.WithdrawAmount = wdAmount;
                budget.RemainAmount = budget.BudgetAmount - budget.WithdrawAmount;
            }

            return budgets;
        }

        #endregion


    }
}