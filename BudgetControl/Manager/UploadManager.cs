using BudgetControl.DAL;
using BudgetControl.Models.Temp;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
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
                // insert into tempbudget
                using (var db = new BudgetContext())
                {
                    using (var sqlBulkCopy = new SqlBulkCopy(db.Database.Connection as SqlConnection))
                    {

                    }
                }

                    return budgetfile;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}