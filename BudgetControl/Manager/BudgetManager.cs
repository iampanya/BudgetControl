using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using BudgetControl.Util;
using BudgetControl.ViewModels;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class BudgetManager
    {
        private BudgetContext _db;

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

        public Budget Get(Guid budgetid)
        {
            using (var budgetRepo = new BudgetRepository())
            {
                return budgetRepo.GetById(budgetid);
            }
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

        #region Export

        public byte[] Export(string costcenterid, string year)
        {
            // get range of budget id
            Guid[] budgetids;
            DataSet ds = new DataSet();

            using (BudgetRepository budgetRep = new BudgetRepository())
            {
                budgetids = budgetRep.Get()
                    .Where(
                        b =>
                            b.CostCenterID == costcenterid
                            && b.Year == year
                    ).Select(b => b.BudgetID).ToArray();
            }

            
            foreach (var budgetid in budgetids)
            {
                var budget = GetBudgetDetail(budgetid);

                var exportModel = new List<BudgetExportViewModel>();
                foreach (var trans in budget.BudgetTransactions)
                {
                    exportModel.Add(new BudgetExportViewModel(trans));
                }

                string tablename = budget.AccountID + "-" + budget.Account.AccountName;
                var dt = Utility.ToDataTable(exportModel, tablename);

                ds.Tables.Add(dt);
            }


            using (MemoryStream mem = new MemoryStream())
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(mem, SpreadsheetDocumentType.Workbook))
                {
                    this.ExportDataSet(ds, spreadsheetDocument);

                    // Close the document.
                    spreadsheetDocument.Close();
                }
                return mem.ToArray();
            }
        }

        private void ExportDataSet(DataSet ds, SpreadsheetDocument workbook)
        {
            var workbookPart = workbook.AddWorkbookPart();

            workbook.WorkbookPart.Workbook = new Workbook();

            workbook.WorkbookPart.Workbook.Sheets = new Sheets();

            foreach (DataTable table in ds.Tables)
            {
                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId =
                        sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                sheets.Append(sheet);

                Row headerRow = new Row();

                List<String> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }


                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (String col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString()); //
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }
            }

            workbookPart.Workbook.Save();
        }


        private Budget GetBudgetDetail(Guid id)
        {
            try
            {
                Budget budget;
                using (BudgetRepository budgetRep = new BudgetRepository())
                {
                    budget = budgetRep.GetById(id);
                    if (budget == null)
                    {
                        throw new Exception("ไม่พบข้อมูลงบประมาณที่เลือก");
                    }
                }

                budget.BudgetTransactions = budget.BudgetTransactions.Where(t => t.Status == RecordStatus.Active).OrderBy(t => t.CreatedAt).ToList();

                decimal previousAmount = 0;
                decimal total = 0;
                foreach (var item in budget.BudgetTransactions)
                {
                    using (var paymentRepo = new PaymentRepository())
                    {
                        item.Payment = paymentRepo.GetById(item.PaymentID);
                    }
                    total += item.Amount;
                    item.PreviousAmount = previousAmount;
                    item.RemainAmount = budget.BudgetAmount - item.PreviousAmount - item.Amount;

                    previousAmount = item.Amount + item.PreviousAmount;
                }
                budget.WithdrawAmount = total;
                budget.RemainAmount = budget.BudgetAmount - budget.WithdrawAmount;

                return budget;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion
    }
}