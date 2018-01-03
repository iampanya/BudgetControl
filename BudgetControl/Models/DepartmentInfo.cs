using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BudgetControl.Models
{
    public class DepartmentInfo
    {
        #region Constructor

        public DepartmentInfo()
        {

        }

        #endregion

        #region Field

        public Guid Id { get; set; }

        #region Dept Code

        [Index(IsUnique = true)]
        public int DeptSap { get; set; }
        
        [Index()]
        public int? DeptUpper { get; set; }

        [StringLength(6)]
        public string DeptStableCode { get; set; }

        [StringLength(15)]
        public string DeptChangeCode { get; set; }

        [StringLength(15)]
        public string DeptOldCode { get; set; }

        #endregion

        #region DeptShort 0 - 7

        [StringLength(100)]
        public string DeptShort { get; set; }

        [StringLength(100)]
        public string DeptShort1 { get; set; }

        [StringLength(100)]
        public string DeptShort2 { get; set; }

        [StringLength(100)]
        public string DeptShort3 { get; set; }

        [StringLength(100)]
        public string DeptShort4 { get; set; }

        [StringLength(100)]
        public string DeptShort5 { get; set; }

        [StringLength(100)]
        public string DeptShort6 { get; set; }

        [StringLength(100)]
        public string DeptShort7 { get; set; }

        #endregion

        #region DeptFull 0 - 7

        [StringLength(500)]
        public string DeptFull { get; set; }

        [StringLength(500)]
        public string DeptFull1 { get; set; }

        [StringLength(500)]
        public string DeptFull2 { get; set; }

        [StringLength(500)]
        public string DeptFull3 { get; set; }

        [StringLength(500)]
        public string DeptFull4 { get; set; }

        [StringLength(500)]
        public string DeptFull5 { get; set; }

        [StringLength(500)]
        public string DeptFull6 { get; set; }

        [StringLength(500)]
        public string DeptFull7 { get; set; }

        #endregion

        #region DeptFullEng 0 - 7

        [StringLength(500)]
        public string DeptFullEng { get; set; }

        [StringLength(500)]
        public string DeptFullEng1 { get; set; }

        [StringLength(500)]
        public string DeptFullEng2 { get; set; }

        [StringLength(500)]
        public string DeptFullEng3 { get; set; }

        [StringLength(500)]
        public string DeptFullEng4 { get; set; }

        [StringLength(500)]
        public string DeptFullEng5 { get; set; }

        [StringLength(500)]
        public string DeptFullEng6 { get; set; }

        [StringLength(500)]
        public string DeptFullEng7 { get; set; }

        #endregion

        [StringLength(100)]
        public string DeptStatus { get; set; }
        [StringLength(100)]
        public string DeptClass { get; set; }
        [StringLength(100)]
        public string DeptArea { get; set; }
        [StringLength(100)]
        public string DeptTel { get; set; }
        public DateTime? DeptEffectDate { get; set; }
        [StringLength(100)]
        public string DeptOrderNo { get; set; }
        public DateTime? DeptEffectOff { get; set; }
        [StringLength(100)]
        public string DeptAt { get; set; }
        [StringLength(100)]
        public string DeptMoiCode { get; set; }

        #region CostCenter

        [Index()]
        [StringLength(10)]
        public string CostCenterCode { get; set; }

        [StringLength(200)]
        public string CostCenterName { get; set; }

        #endregion

        [StringLength(2)]
        public string DeptOrder { get; set; }


        #endregion


    }
}