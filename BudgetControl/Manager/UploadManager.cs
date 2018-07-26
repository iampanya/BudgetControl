using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Temp;
using BudgetControl.Util;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class UploadManager
    {
        #region Constructor

        public UploadManager()
        {

        }

        #endregion

        #region cmd

        private static string cmd_upsert_tempbudget
        {
            get
            {
                return @"
--DECLARE @UploadBy varchar(10)
--SET @UploadBy = 'admin'

MERGE INTO Budget AS TARGET
USING TempBudget AS SOURCE
ON		TARGET.Year = source.Year 
	AND TARGET.AccountID = SOURCE.AccountID
	AND TARGET.CostCenterID = SOURCE.CostCenterID
WHEN MATCHED  AND SOURCE.UploadBy = @UploadBy THEN 
	UPDATE SET	TARGET.BudgetAmount = SOURCE.BudgetAmount
				, TARGET.Status = 1
				, TARGET.ModifiedBy = SOURCE.UploadBy
				, TARGET.ModifiedAt = SOURCE.UploadTime

WHEN NOT MATCHED AND SOURCE.UploadBy = @UploadBy THEN
		INSERT (BudgetID
				, AccountID
				, CostCenterID
				, Sequence
				, Year
				, BudgetAmount
				, WithdrawAmount
				, RemainAmount
				, Status
				, CreatedBy
				, CreatedAt
				, ModifiedBy
				, ModifiedAt
		) 
		VALUES ( SOURCE.Id
				, SOURCE.AccountId
				, SOURCE.CostCenterId
				, 0
				, SOURCE.Year
				, SOURCE.BudgetAmount
				, 0
				, 0
				, 1
				, UploadBy
				, UploadTime
				, UploadBy
				, UploadTime
		);

                ";
            }
        }

        #endregion

        public List<BudgetFileModel> ReadBudgetFile(UploadBudgetModel formdata)
        {
            List<TempBudget> tempBudgets = new List<TempBudget>();
            List<BudgetFileModel> budgetfile = new List<BudgetFileModel>();

            try
            {
                formdata.Validate();

                // 1. Read file line by line
                string[] lines = formdata.FileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (var line in lines)
                {
                    // 1.1 Split each line in to columns
                    string[] columns = line.Split('|');

                    // 1.2 Validate data line
                    if (columns.Length == 6)
                    {
                        // Check second column is digit
                        if (Char.IsDigit(columns[1].Trim()[0]))
                        {
                            // This is data line
                            budgetfile.Add(new BudgetFileModel(columns, formdata.Year));
                        }
                    }
                }     

                // filter by costcenter 
                if (!String.IsNullOrEmpty(formdata.CostCenterBegin))
                {
                    if (String.IsNullOrEmpty(formdata.CostCenterEnd))
                    {
                        formdata.CostCenterEnd = formdata.CostCenterBegin;
                    }
                    budgetfile = budgetfile
                        .Where(b => b.CostCenterID.CompareTo(formdata.CostCenterBegin) >= 0 && b.CostCenterID.CompareTo(formdata.CostCenterEnd) <= 0)
                        .ToList();
                }

                // convert to tempbudget
                budgetfile = budgetfile.OrderBy(b => b.CostCenterID).ThenBy(b => b.AccountID).ToList();
                foreach(var item in budgetfile)
                {
                    int index = tempBudgets
                        .FindIndex(b => 
                            b.CostCenterID == item.CostCenterID && 
                            b.AccountID == item.AccountID &&
                            b.Year == item.Year
                         );
                    if(index >= 0)
                    {
                        tempBudgets[index].BudgetAmount = tempBudgets[index].BudgetAmount + item.Amount;
                    }
                    else
                    {
                        tempBudgets.Add(new TempBudget()
                        {
                            Id = Guid.NewGuid(),
                            AccountID = item.AccountID,
                            AccountName = item.AccountName,
                            CostCenterID = item.CostCenterID,
                            BudgetAmount = item.Amount,
                            Year = item.Year
                        });
                    }
                }
                DateTime now = DateTime.Now;
                tempBudgets.ForEach(i => { i.UploadBy = "admin"; i.UploadTime = now; });

                // Insert into tempbudget
                using (var db = new BudgetContext())
                {
                    db.Database.Connection.Open();

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {

                            // 1. Delete old temp data
                            db.Database.ExecuteSqlCommand("DELETE FROM TempBudget Where UploadBy = @UploadBy", new SqlParameter("@UploadBy", "admin"));

                            // 2. Insert new temp data
                            DataTable dt = Utility.ToDatatable(tempBudgets);
                            using (var sqlBulkCopy = new SqlBulkCopy(db.Database.Connection as SqlConnection, SqlBulkCopyOptions.TableLock, db.Database.CurrentTransaction.UnderlyingTransaction as SqlTransaction))
                            {
                                sqlBulkCopy.BulkCopyTimeout = 2000;
                                sqlBulkCopy.DestinationTableName = "TempBudget";
                                sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                                sqlBulkCopy.ColumnMappings.Add("AccountID", "AccountID");
                                sqlBulkCopy.ColumnMappings.Add("AccountName", "AccountName");
                                sqlBulkCopy.ColumnMappings.Add("CostCenterID", "CostCenterID");
                                sqlBulkCopy.ColumnMappings.Add("Year", "Year");
                                sqlBulkCopy.ColumnMappings.Add("BudgetAmount", "BudgetAmount");
                                sqlBulkCopy.ColumnMappings.Add("UploadBy", "UploadBy");
                                sqlBulkCopy.ColumnMappings.Add("UploadTime", "UploadTime");

                                sqlBulkCopy.WriteToServer(dt);
                            }
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }

                    }
                    
                }

                return budgetfile;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int UpsertBudget(string uploadby)
        {
            int rowaffected = 0;
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BudgetContext"].ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand(cmd_upsert_tempbudget, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@UploadBy", uploadby);
                            rowaffected = cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new SqlCommand("DELETE FROM TempBudget Where UploadBy = @UploadBy", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@UploadBy", uploadby);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return rowaffected;
                    }
                    catch(Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}