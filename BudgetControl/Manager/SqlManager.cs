using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public static class SqlManager
    {
        public static string ConnectionString {
            get
            {
                return ConfigurationManager.ConnectionStrings["BudgetContext"].ConnectionString;
            }
        }
    }
}