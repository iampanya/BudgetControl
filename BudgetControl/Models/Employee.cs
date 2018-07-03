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
    public enum EmployeeGroup
    {
        None = 0,
        Normal = 1, 
        Temporary = 2,
        Terminate = 3,
        Probation = 4
    }

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

            PositionCode = profile.PositionCode;
            PositionDescShort = profile.PositionDescShort;
            PostionDesc = profile.Position;
            LevelCode = profile.LevelCode;
            Email = profile.Email;

            int num;
            DepartmentSap = Int32.TryParse(profile.DepartmentSap, out num) ? num : (int ?)null;

            StaffDate = profile.StaffDate;
            EntryDate = profile.EntryDate;
            RetiredDate = profile.RetiredDate;

            BaCode = profile.BaCode;
            PeaCode = profile.Peacode;

            StatusCode = profile.StatusCode;
            StatusName = profile.Status;

            EmployeeGroup empgroup;
            Group = Enum.TryParse<EmployeeGroup>(profile.GroupId, out empgroup) ? empgroup : EmployeeGroup.None;
            
        }

        #endregion


        [Display(Name = "รหัสพนักงาน")]
        [StringLength(10)]
        public string EmployeeID { get; set; }

        #region Name

        [Display(Name = "คำนำหน้า")]
        [StringLength(64)]
        public string TitleName { get; set; }

        [Display(Name = "ชื่อ")]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Display(Name = "นามสกุล")]
        [StringLength(128)]
        public string LastName { get; set; }

        #endregion

        [Display(Name = "ตำแหน่ง")]
        [StringLength(128)]
        public string JobTitle { get; set; }

        [Display(Name = "ระดับ")]
        public Byte JobLevel { get; set; }

        #region Position and level

        [StringLength(4)]
        public string PositionCode { get; set; }

        [StringLength(100)]
        public string PositionDescShort { get; set; }

        [StringLength(200)]
        public string PostionDesc { get; set; }

        [StringLength(2)]
        public string LevelCode { get; set; }

        #endregion

        [StringLength(200)]
        public string Email { get; set; }

        public string CostCenterID { get; set; }
        
        public int? DepartmentSap { get; set; }

        #region Date Info

        [StringLength(10)]
        public string StaffDate { get; set; }

        [StringLength(10)]
        public string EntryDate { get; set; }

        [StringLength(10)]
        public string RetiredDate { get; set; }

        #endregion

        #region BA - PEA Code

        [StringLength(4)]
        public string BaCode { get; set; }

        [StringLength(6)]
        public string PeaCode { get; set; }

        #endregion

        #region Status & Group

        [StringLength(1)]
        public string StatusCode { get; set; }
        [StringLength(100)]
        public string StatusName { get; set; }

        public EmployeeGroup Group { get; set; }

        #endregion


        [Display(Name = "สิทธิ์การใช้งาน")]
        [StringLength(32)]
        public string Role { get; set; }

        public RecordStatus Status { get; set; }

        #region Relation

        public virtual CostCenter CostCenter { get; set; }

        #endregion

        #region Additional Field
        
        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }

        #endregion

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

            if (PositionCode != newEmp.PositionCode) return true;
            if (LevelCode != newEmp.LevelCode) return true;
            if (Email != newEmp.Email) return true;
            if (DepartmentSap != newEmp.DepartmentSap) return true;
            if (StaffDate != newEmp.StaffDate) return true;
            if (EntryDate != newEmp.EntryDate) return true;
            if (RetiredDate != newEmp.RetiredDate) return true;
            if (BaCode != newEmp.BaCode) return true;
            if (PeaCode != newEmp.PeaCode) return true;
            if (StatusCode != newEmp.StatusCode) return true;
            if (StatusName != newEmp.StatusName) return true;
            if (Group != newEmp.Group) return true;

            return false;
        }

        #endregion
    }
}