using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class Employee : RecordTimeStamp
    {
        [Display(Name = "รหัสพนักงาน")]
        [StringLength(10)]
        public string EmployeeID { get; set; }

        [Display(Name = "คำนำหน้า")]
        [StringLength(64)]
        public string TitleName { get; set; }

        [Display(Name = "ชื่อ")]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Display(Name = "นามสกุล")]
        [StringLength(128)]
        public string LastName { get; set; }

        [Display(Name = "ตำแหน่ง")]
        [StringLength(128)]
        public string JobTitle { get; set; }

        [Display(Name = "ระดับ")]
        public Byte JobLevel { get; set; }
        
        public string CostCenterID { get; set; }


        [Display(Name = "รหัสผ่าน")]
        [StringLength(32)]
        public string Password { get; set; }

        [Display(Name = "สิทธิ์การใช้งาน")]
        [StringLength(32)]
        public string Role { get; set; }

        public RecordStatus Status { get; set; }

        #region Relation

        public virtual CostCenter CostCenter { get; set; }
        
        #endregion

        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }
    }
}