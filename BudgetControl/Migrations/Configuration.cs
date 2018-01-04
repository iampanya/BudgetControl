namespace BudgetControl.Migrations
{
    using BudgetControl.DAL;
    using BudgetControl.Models;
    using BudgetControl.Models.Base;
    using Microsoft.VisualBasic.FileIO;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BudgetControl.DAL.BudgetContext>
    {
        private string basepath = AppDomain.CurrentDomain.BaseDirectory;
        private string path;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BudgetContext context)
        {

            #region Migraiton: Init

            #region Account

            //var accounts = ReadTextAccount();
            //accounts.ForEach(a => context.Accounts.AddOrUpdate(c => c.AccountID, a));
            //context.SaveChanges();


            #endregion

            #region CostCenter

            //var costcenters = ReadTextCostCenter();
            //costcenters.ForEach(a => context.CostCenters.AddOrUpdate(c => c.CostCenterID, a));
            //context.SaveChanges();

            #endregion

            #region PaymentCounter


            //var paymentcounters = GeneratePaymentCounter();
            //paymentcounters.ForEach(p => context.PaymentCounters.AddOrUpdate(a => a.CostCenterID, p));
            //context.SaveChanges();

            #endregion

            #region Budget

            //var budgets = ReadTextBudget();
            //budgets.ForEach(s => context.Budgets.AddOrUpdate(c => new { c.AccountID, c.CostCenterID, c.Year }, s));
            //context.SaveChanges();

            #endregion

            #region Employee

            //var employees = ReadTextEmployee();
            //employees.ForEach(e => context.Employees.AddOrUpdate(c => c.EmployeeID, e));
            //context.SaveChanges();

            #endregion

            #endregion

            #region Migration : MoreEmployeeInfo
            var baInfos = ReadTextBA();
            baInfos.ForEach(b => context.BussinessAreaInfos.AddOrUpdate(c => c.BaCode, b));
            context.SaveChanges();

            var peaInfos = ReadTextPeaInfo();
            peaInfos.ForEach(p => context.PeaInfos.AddOrUpdate(c => c.PeaCode, p));
            context.SaveChanges();

            var positionInfos = ReadTextPositionInfo();
            positionInfos.ForEach(p => context.PositionInfos.AddOrUpdate(c => c.PositionCode, p));
            context.SaveChanges();

            var levelInfos = ReadTextLevelInfo();
            levelInfos.ForEach(l => context.LevelInfos.AddOrUpdate(c => c.LevelCode, l));
            context.SaveChanges();

            #endregion

        }

        #region Get Data from txt file

        #region Account

        private List<Account> ReadTextAccount()
        {
            List<Account> accounts = new List<Account>();
            path = Path.Combine(basepath, @"Data\Account.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        accounts.Add(new Account
                        {
                            AccountID = fields[0],
                            AccountName = fields[1],
                            Status = RecordStatus.Active,
                        });
                    }
                }
            }
            return accounts;
        }

        #endregion

        #region CostCenter

        private List<CostCenter> ReadTextCostCenter()
        {
            List<CostCenter> costcenters = new List<CostCenter>();
            path = Path.Combine(basepath, @"Data\CostCenter.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        costcenters.Add(new CostCenter
                        {
                            CostCenterID = fields[0].Trim(),
                            CostCenterName = fields[1],
                            ShortName = fields[2],
                            Status = RecordStatus.Active
                        });
                    }
                }
            }
            return costcenters;
        }


        #endregion

        #region PaymentCounter

        private List<PaymentCounter> GeneratePaymentCounter()
        {
            List<PaymentCounter> paymentcounters = new List<PaymentCounter>();
            var costcenters = ReadTextCostCenter();
            foreach (var item in costcenters)
            {
                string[] costcentersplit = item.ShortName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                string shortcode;
                if (costcentersplit[costcentersplit.Length - 1].Length < 3)
                {
                    var l = costcentersplit.Length;
                    shortcode = costcentersplit[l - 2] + "." + costcentersplit[l - 1];
                }
                else
                {
                    var test = costcentersplit[costcentersplit.Length - 1].Split(' ');
                    shortcode = test[test.Length - 1];
                }

                paymentcounters.Add(new PaymentCounter()
                {
                    CostCenterID = item.CostCenterID,
                    Year = "2560",
                    ShortCode = shortcode,
                    Number = 0
                });
            }
            return paymentcounters;
        }

        #endregion

        #region Employee

        private List<Employee> ReadTextEmployee()
        {
            List<Employee> employees = new List<Employee>();
            path = Path.Combine(basepath, @"Data\Employee_20171015.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        employees.Add(new Employee
                        {
                            EmployeeID = fields[0],
                            TitleName = fields[1],
                            FirstName = fields[2],
                            LastName = fields[3],
                            JobTitle = fields[4],
                            JobLevel = Byte.Parse(fields[5]),
                            CostCenterID = fields[6],
                            Status = RecordStatus.Active
                        });
                    }
                }
            }
            return employees;
        }

        #endregion

        #region Budget

        private List<Budget> ReadTextBudget()
        {
            List<Budget> budgets = new List<Budget>();
            path = Path.Combine(basepath, @"Data\Budget.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        budgets.Add(new Budget
                        {
                            BudgetID = Guid.NewGuid(),
                            AccountID = fields[0],
                            CostCenterID = fields[1],
                            Year = "2560",
                            BudgetAmount = decimal.Parse(fields[2]),
                            Status = BudgetStatus.Active
                        });
                    }
                }
            }
            return budgets;
        }

        #endregion

        #endregion

        #region  Method for migration : MoreEmployeeInfo

        private List<BusinessAreaInfo> ReadTextBA()
        {
            List<BusinessAreaInfo> baInfos = new List<BusinessAreaInfo>();
            path = Path.Combine(basepath, @"Data\BAInfo.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        var baInfo = new BusinessAreaInfo();
                        baInfo.Id = Guid.NewGuid();
                        baInfo.BaCode = fields[0].Trim();
                        baInfo.BaName = fields[1].Trim();
                        baInfo.NewCreateTimeStamp("Seed");
                        baInfos.Add(baInfo);
                    }
                }
            }
            return baInfos;
        }

        private List<PeaInfo> ReadTextPeaInfo()
        {
            List<PeaInfo> peainfos = new List<PeaInfo>();
            path = Path.Combine(basepath, @"Data\PeaCodeInfo.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        var peaInfo = new PeaInfo();
                        peaInfo.Id = Guid.NewGuid();
                        peaInfo.PeaCode = fields[0].Trim().ToUpper();
                        peaInfo.PeaShortName = fields[1].Trim();
                        peaInfo.PeaName = fields[2].Trim();
                        peaInfo.NewCreateTimeStamp("Seed");
                        peainfos.Add(peaInfo);
                    }
                }
            }


            return peainfos;
        }

        private List<LevelInfo> ReadTextLevelInfo()
        {
            List<LevelInfo> levelInfos = new List<LevelInfo>();
            path = Path.Combine(basepath, @"Data\LevelCode.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        var levelInfo = new LevelInfo();
                        levelInfo.Id = Guid.NewGuid();
                        levelInfo.LevelCode = fields[0].Trim();
                        levelInfo.LevelDesc = fields[1].Trim();
                        levelInfo.NewCreateTimeStamp("Seed");
                        levelInfos.Add(levelInfo);
                    }
                }
            }

            return levelInfos;
        }

        private List<PositionInfo> ReadTextPositionInfo()
        {
            List<PositionInfo> positions = new List<PositionInfo>();
            path = Path.Combine(basepath, @"Data\Position.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        var position = new PositionInfo();
                        position.Id = Guid.NewGuid();
                        position.PositionCode = fields[0].Trim();
                        position.PositionDescShort = fields[1].Trim();
                        position.PostionDesc = fields[2].Trim();
                        position.NewCreateTimeStamp("Seed");
                        positions.Add(position);
                    }
                }
            }

            return positions;
        }

        #endregion

    }
}
