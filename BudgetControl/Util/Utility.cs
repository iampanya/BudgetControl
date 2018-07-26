using BudgetControl.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BudgetControl.Util
{
    public  static class Utility
    {
        public static string ParseToJson(object obj)
        {
            //TODO Add try catch here
            try
            {
                var jsonresult = JsonConvert.SerializeObject(
                        obj,
                        Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }
                    );

                return jsonresult;
            }
            catch (Exception ex) {
                ReturnObject returnobj = new ReturnObject();
                returnobj.SetError(ex.Message);

                var jsonresult = JsonConvert.SerializeObject(
                        returnobj,
                        Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }
                    );
                return jsonresult;
            }

        }

        public static string ToDateText(DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    return "-";
                }
                else
                {
                    return ((DateTime)date).ToString("d MMM yyyy", new CultureInfo("th-TH"));
                }
            }
            catch(Exception ex)
            {
                return "-";
            }
        }

        public static DataTable ToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}