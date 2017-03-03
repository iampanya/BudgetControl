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

            #region BudgetTransaction
            //budgets.ForEach(b => transactions.Add(new BudgetTransaction { 
            //    BudgetTransactionID = Guid.NewGuid(),
            //    BudgetID = b.BudgetID,
            //    PaymentID = null,
            //    Description = "Define Budget",
            //    Amount = b.BudgetAmount,
            //    PreviousAmount = 0,
            //    RemainAmount = b.BudgetAmount,
            //    RefID = null,
            //    Type = TransactionType.Definition
            //}));

            //transactions.ForEach(t => context.BudgetTransactions.AddOrUpdate(c => c.BudgetID, t));
            //context.SaveChanges();

            #endregion

            #region Employee
            path = Path.Combine(basepath, @"Data\Employee.txt");

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
                            Password = fields[7],
                            Status = RecordStatus.Active
                        });
                    }
                }
            }
            employees.ForEach(e => context.Employees.AddOrUpdate(c => c.EmployeeID, e));
            context.SaveChanges();
            #endregion

            #region User

            employees.ForEach(e =>
            {
                users.Add(new User
                {
                    EmployeeID = e.EmployeeID,
                    UserName = e.EmployeeID,
                    Password = e.Password,
                    Role = UserRole.Normal,
                    Status = RecordStatus.Active
                });
            });
            
            users.ForEach(u => context.Users.AddOrUpdate(c => c.UserID, u));
            context.SaveChanges();

            #endregion

            #region Payment
            //employees.ForEach(e =>
            //{
            //    payments.Add(GeneratePayment(e));
            //    payments.Add(GeneratePayment(e));
            //});

            //payments.ForEach(p => context.Payments.AddOrUpdate(a => a.Sequence, p));
            //context.SaveChanges();

            #endregion


            #region Statement
            //List<Statement> statements = new List<Statement>();
            //payments.ForEach(p => statements.AddRange(GenerateStatement(p)));

            //statements.ForEach(s => context.Statements.AddOrUpdate(a => a.StatementID, s));
            //context.SaveChanges();


            #endregion
        }

        private Payment GeneratePayment(Employee employee)
        {
            var payment = new Payment();

            //var requester = employees.FirstOrDefault(e => e.CostCenterID == costcenter);
            var sequence = payments.Where(p => p.CostCenterID == employee.CostCenterID).Count()+1;

            payment.PaymentID = Guid.NewGuid();
            payment.Year = "2559";
            payment.CostCenterID = employee.CostCenterID;
            payment.TotalAmount = 0;
            payment.RequestBy = employee.EmployeeID;
            payment.ControlBy = employee.EmployeeID;
            payment.Sequence = sequence;
            payment.PaymentDate = DateTime.Today;
            payment.Description = "เบี้ยเลี้ยง " + employee.FirstName;
            payment.Status = RecordStatus.Active;
            RecordTimeStamp rt = new RecordTimeStamp();
            rt.NewCreateTimeStamp();
            payment.SetCreateTimeStamp(rt);
            return payment;
        }
        private List<Statement> GenerateStatement(Payment payment)
        {
            List<Statement> result = new List<Statement>();
            var random = new Random();
            var budgetbycostcenter = budgets.Where(b => b.CostCenterID == payment.CostCenterID).ToList();
            
            budgetbycostcenter.ForEach(b => {
                var value = random.Next(100, 701);
                result.Add(new Statement
                {
                    StatementID = Guid.NewGuid(),
                    PaymentID = payment.PaymentID,
                    BudgetID = b.BudgetID,
                    WithdrawAmount = value,
                    Status = RecordStatus.Active,
                    CreatedAt = payment.CreatedAt,
                    CreatedBy = payment.CreatedBy,
                    ModifiedAt = payment.ModifiedAt,
                    ModifiedBy = payment.ModifiedBy
                });
            });

            return result;
        }

    }
}
