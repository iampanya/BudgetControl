using BudgetControl.DAL;
using BudgetControl.Sessions;
using BudgetControl.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models.Base
{

    public enum RecordStatus
    {
        Active = 0x01,
        Remove = 0x02
    }

    public class RecordTimeStamp
    {
        #region Constructor

        public RecordTimeStamp() { }

        #endregion

        #region Fields

        [ScaffoldColumn(false)]
        [Display(Name = "สร้างโดย")]
        [StringLength(128)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "สร้างเมื่อ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedAt { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "แก้ไขล่าสุดโดย")]
        [StringLength(128)]
        public string ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "แก้ไขล่าสุดเมื่อ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedAt { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "ลบโดย")]
        [StringLength(128)]
        public string DeletedBy { get; set; }

        [Index()]
        [ScaffoldColumn(false)]
        [Display(Name = "ลบเมื่อ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeletedAt { get; set; }

        #endregion


        #region NotMapped

        [NotMapped]
        public string CreatedByName { get; set; }
        
        [NotMapped]
        public string ModifiedByName { get; set; }
        
        [NotMapped]
        public string DeletedByName { get; set; }


        public string CreatedDate
        {
            get
            {
                return Utility.ToDateText(CreatedAt);
            }
        }

        public string CreatedTime
        {
            get
            {
                return CreatedAt == null ? "-" : ((DateTime)CreatedAt).ToString("HH:mm:ss");
            }
        }

        public string ModifiedDate
        {
            get
            {
                return Utility.ToDateText(ModifiedAt);
            }
        }

        public string ModifiedTime
        {
            get
            {
                return ModifiedAt == null ? "-" : ((DateTime)ModifiedAt).ToString("HH:mm:ss");
            }
        }

        public string DeletedDate
        {
            get
            {
                return Utility.ToDateText(DeletedAt);
            }
        }

        public string DeletedTime
        {
            get
            {
                return DeletedAt == null ? "-" : ((DateTime)DeletedAt).ToString("HH:mm:ss");
            }
        }

        #endregion

        #region Methods

        public void NewCreateTimeStamp(string createdby="")
        {
            //Prevent exception when nuget: update-database 
            try
            {
                CreatedBy = AuthManager.GetCurrentUser().EmployeeID;
            }
            catch (Exception)
            {
                CreatedBy = String.IsNullOrEmpty(createdby) ? "Anonymous" : createdby;
            }

            CreatedAt = DateTime.Now;
            ModifiedBy = CreatedBy;
            ModifiedAt = CreatedAt;
        }

        public void NewModifyTimeStamp(string modifiedby="")
        {
            try
            {
                ModifiedBy = AuthManager.GetCurrentUser().EmployeeID;
            }
            catch (Exception)
            {
                ModifiedBy = String.IsNullOrEmpty(modifiedby) ? "Anonymous" : modifiedby;
            }

            ModifiedAt = DateTime.Now;
            DeletedAt = null;
            DeletedBy = null;

        }

        public void NewDeleteTimestamp(string deletedby="")
        {
            try
            {
                DeletedBy = AuthManager.GetCurrentUser().EmployeeID;
            }
            catch (Exception)
            {
                DeletedBy = String.IsNullOrEmpty(deletedby) ? "Anonymous" : deletedby;
            }

            DeletedAt = DateTime.Now;
        }

        public void SetCreateTimeStamp(RecordTimeStamp timestamp)
        {
            CreatedBy = timestamp.CreatedBy;
            CreatedAt = timestamp.CreatedAt;
            SetModifiedTimeStamp(timestamp);
        }

        public void SetModifiedTimeStamp(RecordTimeStamp timestamp)
        {
            ModifiedBy = timestamp.ModifiedBy;
            ModifiedAt = timestamp.ModifiedAt;
        }

        public void GetRecordsNameInfo()
        {
            string[] empId = new string[] { CreatedBy, ModifiedBy, DeletedBy };
            empId = empId.Distinct().ToArray();

            List<Employee> employees;
            using(var db = new BudgetContext())
            {
                employees = db.Employees.Where(e => empId.Contains(e.EmployeeID)).ToList();
            }

            if(employees != null)
            {
                var createdby = employees.FirstOrDefault(e => e.EmployeeID == CreatedBy);
                CreatedByName = createdby == null ? CreatedBy : String.Join(" ", createdby.FirstName, createdby.LastName);

                var modifiedby = employees.FirstOrDefault(e => e.EmployeeID == ModifiedBy);
                ModifiedByName = modifiedby == null ? ModifiedBy : String.Join(" ", modifiedby.FirstName, modifiedby.LastName);

                var deletedby = employees.FirstOrDefault(e => e.EmployeeID == DeletedBy);
                DeletedByName = deletedby == null ? DeletedBy : String.Join(" ", deletedby.FirstName, deletedby.LastName);
            }
        }


        #endregion

    }
}