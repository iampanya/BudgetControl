using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BudgetControl.DAL
{
    public class BudgetContext : DbContext
    {
        public BudgetContext() : base("BudgetContext")
        {
            this.Configuration.ProxyCreationEnabled = false;
            //this.Database.CommandTimeout = 180;
        }
        public BudgetContext(bool isProxyEnabled) : base("BudgetContext")
        {
            this.Configuration.ProxyCreationEnabled = isProxyEnabled;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetTransaction> BudgetTransactions { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<PaymentCounter> PaymentCounters { get; set; }
        public DbSet<Contractor> Contractor { get; set; }

        public DbSet<DepartmentInfo> DepartmentInfos { get; set; }
        public DbSet<BusinessAreaInfo> BussinessAreaInfos { get; set; }
        public DbSet<LevelInfo> LevelInfos { get; set; }
        public DbSet<PeaInfo> PeaInfos { get; set; }

        public DbSet<AuthorizeCostCenter> AuthorizeCostCenters { get; set; }
        public DbSet<WorkingCC> WorkingCCs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Payment>()
            //    .HasRequired(p => p.Requester)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Payment>()
            //    .HasRequired(p => p.Controller)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}