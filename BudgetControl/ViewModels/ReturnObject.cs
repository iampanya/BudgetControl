using BudgetControl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.ViewModels
{
    public class ReturnObject
    {
        public ReturnObject()
        {

        }

        public ReturnObject(bool issuccess, string message, object result)
        {
            this.isSuccess = issuccess;
            this.Message = message;
            this.Result = result;
        }

        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }


        public void SetSuccess(object result)
        {
            this.isSuccess = true;
            this.Message = string.Empty;
            this.Result = result;
        }

        public void SetError(string message)
        {
            this.isSuccess = false;
            this.Message = message;
            this.Result = null;
        }

        public string ToJson()
        {
            return Utility.ParseToJson(this);
        }
    }
}