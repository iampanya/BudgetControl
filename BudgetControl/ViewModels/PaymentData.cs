using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class PaymentData
    {
        public Payment Payment { get; set; }

        public List<Statement> Statements { get; set; }

        public ICollection<Budget> Budgets { get; set; }

    }
}