using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BudgetControl.Extensions
{
    public static class ReaderExtensions
    {
        public static DateTime? GetNullableDateTime(this SqlDataReader reader, string name)
        {
            var col = reader.GetOrdinal(name);
            return reader.IsDBNull(col) ?
                        (DateTime?)null :
                        (DateTime?)reader.GetDateTime(col);
        }
    }
}