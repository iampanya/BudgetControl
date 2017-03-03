using BudgetControl.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

    }
}