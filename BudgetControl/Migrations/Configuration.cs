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
    using System.Globalization;
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

            var levelInfos = ReadTextLevelInfo();
            levelInfos.ForEach(l => context.LevelInfos.AddOrUpdate(c => c.LevelCode, l));
            context.SaveChanges();

            var departments = ReadTextDepartment();
            departments.ForEach(d => context.DepartmentInfos.AddOrUpdate(c => c.DeptSap, d));
            context.SaveChanges();

            var costcenters = ReadTextCostCenter_2018();
            costcenters.ForEach(a => context.CostCenters.AddOrUpdate(c => c.CostCenterID, a));
            context.SaveChanges();

            var employees = ReadTextEmployeeIDM();
            employees.ForEach(e => context.Employees.AddOrUpdate(c => c.EmployeeID, e));
            context.SaveChanges();

            var authorizes = InitAuthorizeCostCenter();
            authorizes.ForEach(e => context.AuthorizeCostCenters.AddOrUpdate(c => new { c.CostCenterCode, c.Condition, c.CCAStart, c.CCAEnd }, e));
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

        private List<DepartmentInfo> ReadTextDepartment()
        {
            List<DepartmentInfo> departments = new List<DepartmentInfo>();
            path = Path.Combine(basepath, @"Data\Department.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        var dept = new DepartmentInfo();
                        dept.Id = Guid.NewGuid();
                        dept.DeptSap = Int32.Parse(fields[0].ToString());
                        dept.DeptUpper = Int32.Parse(fields[1].ToString());
                        dept.DeptStableCode = fields[2].ToString();
                        dept.DeptChangeCode = fields[3].ToString();
                        dept.DeptOldCode = fields[4].ToString();

                        dept.DeptShort = fields[5].ToString();
                        dept.DeptShort1 = fields[6].ToString();
                        dept.DeptShort2 = fields[7].ToString();
                        dept.DeptShort3 = fields[8].ToString();
                        dept.DeptShort4 = fields[9].ToString();
                        dept.DeptShort5 = fields[10].ToString();
                        dept.DeptShort6 = fields[11].ToString();
                        dept.DeptShort7 = fields[12].ToString();

                        dept.DeptFull = fields[13].ToString();
                        dept.DeptFull1 = fields[14].ToString();
                        dept.DeptFull2 = fields[15].ToString();
                        dept.DeptFull3 = fields[16].ToString();
                        dept.DeptFull4 = fields[17].ToString();
                        dept.DeptFull5 = fields[18].ToString();
                        dept.DeptFull6 = fields[19].ToString();
                        dept.DeptFull7 = fields[20].ToString();

                        dept.DeptFullEng = fields[21].ToString() == "NULL" ? null : fields[21].ToString();
                        dept.DeptFullEng1 = fields[22].ToString();
                        dept.DeptFullEng2 = fields[23].ToString();
                        dept.DeptFullEng3 = fields[24].ToString();
                        dept.DeptFullEng4 = fields[25].ToString();
                        dept.DeptFullEng5 = fields[26].ToString();
                        dept.DeptFullEng6 = fields[27].ToString();
                        dept.DeptFullEng7 = fields[28].ToString();

                        dept.DeptStatus = fields[29].ToString();
                        dept.DeptClass = fields[30].ToString();
                        dept.DeptArea = fields[31].ToString();
                        dept.DeptTel = fields[32].ToString();
                        dept.DeptEffectDate = DateTime.ParseExact(fields[33].ToString().Substring(0, 10), "yyyy-MM-dd", CultureInfo.CurrentCulture);
                        dept.DeptOrderNo = fields[34].ToString();
                        dept.DeptEffectOff = fields[35].ToString().ToUpper() == "NULL" ? (DateTime?)null: DateTime.ParseExact(fields[35].ToString(), "yyyy-MM-dd", CultureInfo.CurrentCulture);
                        dept.DeptAt = fields[36].ToString();
                        dept.DeptMoiCode = fields[37].ToString();
                        dept.CostCenterCode = fields[38].ToString();
                        dept.CostCenterName = fields[39].ToString();
                        dept.DeptOrder = fields[40].ToString();

                        dept.NewCreateTimeStamp("Seed");
                        departments.Add(dept);
                    }
                }
            }

            return departments;
        }

        private List<Employee> ReadTextEmployeeIDM()
        {
            List<Employee> employees = new List<Employee>();
            path = Path.Combine(basepath, @"Data\Employee_20180101.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        var emp = new Employee();
                        emp.EmployeeID = fields[0].ToString().TrimStart(new char[] { '0' });
                        emp.TitleName = fields[1].ToString();
                        emp.FirstName = fields[2].ToString();
                        emp.LastName = fields[3].ToString();

                        emp.PositionCode = fields[4].ToString();
                        emp.PositionDescShort = fields[5].ToString();
                        emp.PostionDesc = fields[6].ToString();
                        emp.LevelCode = fields[7].ToString();
                        emp.Email = fields[8].ToString();
                        int deptsap;
                        emp.DepartmentSap = Int32.TryParse(fields[9].ToString(), out deptsap) ? (int?)deptsap : null;
                        emp.StaffDate = fields[10].ToString();
                        emp.EntryDate = fields[11].ToString();
                        emp.RetiredDate = fields[12].ToString();
                        emp.BaCode = fields[13].ToString();
                        emp.PeaCode = fields[14].ToString();
                        emp.StatusCode = fields[15].ToString();
                        emp.StatusName = fields[16].ToString();

                        EmployeeGroup employeeGroup;
                        emp.Group = Enum.TryParse<EmployeeGroup>(fields[17].ToString(), out employeeGroup) ? employeeGroup : EmployeeGroup.None;
                        emp.CostCenterID = fields[18].ToString();

                        emp.JobTitle = emp.PositionDescShort;
                        emp.JobLevel = 9;
                        emp.Status = RecordStatus.Active;
                        emp.NewModifyTimeStamp("Seed");


                        employees.Add(emp);
                    }
                }
            }

            return employees;
        }

        private List<CostCenter> ReadTextCostCenter_2018()
        {
            List<CostCenter> costcenters = new List<CostCenter>();
            path = Path.Combine(basepath, @"Data\CostCenter_exclude.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        var costcenter = new CostCenter();
                        //if (!fields[1].ToString().Contains("ยกเลิก"))
                        //{
                            costcenter.CostCenterID = fields[0].ToString();
                            costcenter.CostCenterName = fields[1].ToString();
                            costcenter.ShortName = fields[1].ToString().Split(new char[] { '-' })[0];
                            costcenter.Status = RecordStatus.Active;
                            costcenter.NewCreateTimeStamp("Seed");
                            costcenters.Add(costcenter);
                        //}
                    }
                }
            }

            return costcenters;

        }

        private List<AuthorizeCostCenter> InitAuthorizeCostCenter()
        {
            List<AuthorizeCostCenter> authorizes = new List<AuthorizeCostCenter>();

            path = Path.Combine(basepath, @"Data\Authorize_CostCenter.txt");

            using (StreamReader reader = new StreamReader(path))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("\t");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        var authorize = new AuthorizeCostCenter();
                        authorize.Id = Guid.NewGuid();
                        authorize.AuthorizeType = AuthorizeType.CostCenter;
                        authorize.CostCenterCode = fields[1].ToString();
                        authorize.Condition = ConditionType.ExactMatch;
                        authorize.CCAStart = fields[3].ToString();
                        authorize.CCAEnd = fields[4].ToString();

                        authorize.CanView = true;
                        authorize.CanEdit = true;
                        authorize.CanWithdraw = true;
                        authorize.CanDelete = true;

                        //authorize.CanView = Boolean.Parse(fields[5].ToString());
                        //authorize.CanEdit = Boolean.Parse(fields[6].ToString());
                        //authorize.CanWithdraw = Boolean.Parse(fields[7].ToString());
                        //authorize.CanDelete = Boolean.Parse(fields[8].ToString());

                        authorize.NewCreateTimeStamp("Seed");
                        authorizes.Add(authorize);
                    }
                }
            }

            return authorizes;
        }

        #endregion

    }
}
