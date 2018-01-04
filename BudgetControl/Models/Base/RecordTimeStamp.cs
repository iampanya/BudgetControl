using BudgetControl.Sessions;
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
        
        #region Methods

        public void NewCreateTimeStamp()
        {
            //Prevent exception when nuget: update-database 
            try
            {
                CreatedBy = AuthManager.GetCurrentUser().EmployeeID;
            }
            catch (Exception)
            {
                CreatedBy = "Anonymous";
            }

            CreatedAt = DateTime.Now;
            ModifiedBy = CreatedBy;
            ModifiedAt = CreatedAt;
        }

        public void NewModifyTimeStamp()
        {
            try
            {
                ModifiedBy = AuthManager.GetCurrentUser().EmployeeID;
            }
            catch (Exception)
            {
                ModifiedBy = "Anonymous";
            }

            ModifiedAt = DateTime.Now;
            DeletedAt = null;
            DeletedBy = null;

        }

        public void NewDeleteTimestamp()
        {
            try
            {
                DeletedBy = AuthManager.GetCurrentUser().EmployeeID;
            }
            catch (Exception)
            {
                DeletedBy = "Anonymous";
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

        #endregion

    }
}