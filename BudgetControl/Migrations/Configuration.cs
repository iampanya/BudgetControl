namespace BudgetControl.Migrations
{
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
        List<Employee> employees = new List<Employee>();
        List<User> users = new List<User>();
        List<Account> accounts = new List<Account>();
        List<CostCenter> costcenters = new List<CostCenter>();
        List<Budget> budgets = new List<Budget>();
        List<BudgetTransaction> transactions = new List<BudgetTransaction>();
        List<Payment> payments = new List<Payment>();

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BudgetControl.DAL.BudgetContext context)
        {
            string basepath = AppDomain.CurrentDomain.BaseDirectory;
            string path;


            #region Account

            //path = Path.Combine(basepath, @"Data\Account.txt");


            //using (StreamReader reader = new StreamReader(path))
            //{
            //    using (TextFieldParser parser = new TextFieldParser(reader))
            //    {
            //        parser.TextFieldType = FieldType.Delimited;
            //        parser.SetDelimiters("\t");
            //        while (!parser.EndOfData)
            //        {
            //            string[] fields = parser.ReadFields();
            //            accounts.Add(new Account
            //            {
            //                AccountID = fields[0],
            //                AccountName = fields[1],
            //                Status = RecordStatus.Active,
            //            });
            //        }
            //    }
            //}
            //accounts.ForEach(a => context.Accounts.AddOrUpdate(c => c.AccountID, a));
            //context.SaveChanges();


            #endregion

            #region CostCenter
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
            costcenters.ForEach(a => context.CostCenters.AddOrUpdate(c => c.CostCenterID, a));
            context.SaveChanges();

            #endregion

            #region PaymentCounter

            //List<PaymentCounter> paymentcounters = new List<PaymentCounter>();
            
            //foreach(var item in costcenters)
            //{
            //    string[] costcentersplit = item.ShortName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            //    string shortcode;
            //    if(costcentersplit[costcentersplit.Length - 1].Length < 3)
            //    {
            //        var l = costcentersplit.Length;
            //        shortcode = costcentersplit[l - 2] + "." + costcentersplit[l - 1];
            //    }
            //    else
            //    {
            //        var test = costcentersplit[costcentersplit.Length - 1].Split(' ');
            //        shortcode = test[test.Length - 1];
            //    }
                
            //    paymentcounters.Add(new PaymentCounter()
            //    {
            //        CostCenterID = item.CostCenterID,
            //        Year = "2560",
            //        ShortCode = shortcode,
            //        Number = 0
            //    });
            //}

            //paymentcounters.ForEach(p => context.PaymentCounters.AddOrUpdate(a => a.CostCenterID, p));
            //context.SaveChanges();

            #endregion

            #region Budget
            //path = Path.Combine(basepath, @"Data\Budget.txt");


            //using (StreamReader reader = new StreamReader(path))
            //{
            //    using (TextFieldParser parser = new TextFieldParser(reader))
            //    {
            //        parser.TextFieldType = FieldType.Delimited;
            //        parser.SetDelimiters("\t");
            //        while (!parser.EndOfData)
            //        {
            //            string[] fields = parser.ReadFields();
            //            budgets.Add(new Budget
            //            {
            //                BudgetID = Guid.NewGuid(),
            //                AccountID = fields[0],
            //                CostCenterID = fields[1],
            //                Year = "2560",
            //                BudgetAmount = float.Parse(fields[2]),
            //                Status = BudgetStatus.Active
            //            });
            //        }
            //    }
            //}

            //budgets.ForEach(s => context.Budgets.AddOrUpdate(c => new { c.AccountID, c.CostCenterID, c.Year }, s));
            //context.SaveChanges();
            #endregion

            #region Employee
            path = Path.Combine(basepath, @"Data\Employee_20171015.txt");

            //List<Employee> employees = new List<Employee>();
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
            employees.ForEach(e => context.Employees.AddOrUpdate(c => c.EmployeeID, e));
            context.SaveChanges();
            #endregion

            #region User

            //employees.ForEach(e =>
            //{
            //    users.Add(new User
            //    {
            //        EmployeeID = e.EmployeeID,
            //        UserName = e.EmployeeID,
            //        Password = e.Password,
            //        Role = UserRole.Normal,
            //        Status = RecordStatus.Active
            //    });
            //});
            
            //users.ForEach(u => context.Users.AddOrUpdate(c => c.UserName, u));
            //context.SaveChanges();

            #endregion
            
        }
        

    }
}
