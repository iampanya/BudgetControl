using BudgetControl.Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetControl.Models.Base
{

    public enum RecordStatus
    {
        Active,
        Remove
    }

    public class RecordTimeStamp
    {
        public RecordTimeStamp() {
            ////Prevent exception when nuget: update-database 
            //try
            //{
            //    CreatedBy = SessionManager.GetSessionUserName();
            //}
            //catch (Exception ex)
            //{
            //    CreatedBy = "Anonymous";
            //}

            //CreatedAt = DateTime.Now;
            //ModifiedBy = CreatedBy;
            //ModifiedAt = CreatedAt;
        }

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

        public void NewCreateTimeStamp()
        {
            //Prevent exception when nuget: update-database 
            try
            {
                this.CreatedBy = AuthManager.GetCurrentUser().EmployeeID;
            }
            catch (Exception ex)
            {
                this.CreatedBy = "Anonymous";
            }

            this.CreatedAt = DateTime.Now;
            this.ModifiedBy = this.CreatedBy;
            this.ModifiedAt = this.CreatedAt;
        }

        public void NewModifyTimeStamp()
        {
            try
            {
                this.ModifiedBy = AuthManager.GetCurrentUser().EmployeeID;
            }
            catch (Exception ex)
            {
                this.ModifiedBy = "Anonymous";
            }

            this.ModifiedAt = DateTime.Now;
        }

        public void SetCreateTimeStamp(RecordTimeStamp timestamp)
        {
            this.CreatedBy = timestamp.CreatedBy;
            this.CreatedAt = timestamp.CreatedAt;
            SetModifiedTimeStamp(timestamp);
        }

        public void SetModifiedTimeStamp(RecordTimeStamp timestamp)
        {
            this.ModifiedBy = timestamp.ModifiedBy;
            this.ModifiedAt = timestamp.ModifiedAt;
        }

        //public void SetCreatedTimeStamp(string createdBy, DateTime? createdAt)
        //{
        //    this.CreatedBy = SessionManager.GetSessionUserName();
        //    this.CreatedAt = DateTime.Now;
        //    this.SetModifiedTimeStamp(this.CreatedBy, this.CreatedAt);
        //}

        //public void SetModifiedTimeStamp(string modifiedBy, DateTime? modifiedAt)
        //{
        //    this.ModifiedBy = modifiedBy;
        //    this.ModifiedAt = modifiedAt;
        //}
    }
}