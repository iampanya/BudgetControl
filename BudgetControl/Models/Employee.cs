using BudgetControl.IdmEmployeeServices;
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
        #region Constructor

        public Employee()
        {

        }

        public Employee(EmployeeProfile profile)
        {
            EmployeeID = profile.EmployeeId.TrimStart(new char[] { '0' });
            TitleName = profile.TitleFullName;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            JobTitle = profile.PositionDescShort == profile.LevelDesc ? profile.PositionDescShort : profile.PositionDescShort + profile.LevelDesc;
            JobLevel = 9;
            CostCenterID = profile.CostCenterCode;
            Status = RecordStatus.Active;
        }

        #endregion


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

        #region Methods

        public bool HasChange(Employee newEmp)
        {
            if (EmployeeID.TrimStart(new char[] { '0' }) != newEmp.EmployeeID.TrimStart(new char[] { '0' }))
                return true;
            if (TitleName != newEmp.TitleName) return true;
            if (FirstName != newEmp.FirstName) return true;
            if (LastName != newEmp.LastName) return true;
            if (JobTitle != newEmp.JobTitle) return true;
            if (JobLevel != newEmp.JobLevel) return true;
            if (CostCenterID != newEmp.CostCenterID) return true;
            if (Status != newEmp.Status) return true;

            return false;
        }

        #endregion
    }
}