using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public enum UserRole
    {
        Normal = 0x01, 
        SuperUser = 0x02,
        Admin = 0x04
    }

    public class User : RecordTimeStamp
    {
        public int UserID { get; set; }
        public string EmployeeID { get; set; }

        [StringLength(10)]
        public string UserName { get; set; }
        [StringLength(32)]
        public string Password { get; set; }
        public Guid? Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public UserRole Role { get; set; }
        public RecordStatus Status { get; set; }
        public DateTime? LastLogin { get; set; }

        [StringLength(256)]
        public string Remark { get; set; }

        #region Relation

        public virtual Employee Employee { get; set; }

        #endregion
    }
}