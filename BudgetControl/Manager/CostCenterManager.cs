using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class CostCenterManager
    {
        private BudgetContext _db;

        #region Constructor

        public CostCenterManager()
        {
            _db = new BudgetContext();
        }

        public CostCenterManager(BudgetContext context)
        {
            _db = context;
        }

        #endregion

        #region Read

        public CostCenter GetByID(string id)
        {
            return _db.CostCenters.Find(id);
        }

        public List<CostCenter> GetWithChildren(string costcenterid)
        {
            List<CostCenter> costcenters = new List<CostCenter>();

            if (String.IsNullOrEmpty(costcenterid))
            {
                throw new Exception("รหัสศูนย์ต้นทุนไม่ถูกต้อง");
            }

            using (SqlConnection conn = new SqlConnection(SqlManager.ConnectionString))
            {
                conn.Open();
                string cmd_get_with_children = @"
                    DECLARE @CostCenterId varchar(12) = 'H305101020'
                    DECLARE @DeptSap int
                    SELECT @DeptSap = dept_sap FROM  V_department where cost_center_code = @CostCenterCode

                    ;WITH DeptTree as(
                        SELECT * FROM V_department
                        WHERE dept_sap = @DeptSap

                        UNION ALL

                        SELECT d.* FROM V_department d
                        INNER JOIN DeptTree x ON d.dept_upper= x.dept_sap
                    )
                    SELECT * 
                    FROM CostCenter 
                    WHERE CostCenterID IN 
	                    (
		                    SELECT cost_center_code FROM DeptTree 
	                    )
                    AND [Status] = 1
                    ORDER BY CostCenterID
                ";

                using (SqlCommand cmd = new SqlCommand(cmd_get_with_children, conn))
                {
                    cmd.Parameters.AddWithValue("CostCenterCode", costcenterid);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CostCenter costcenter = ConvertReaderToCostCenter(reader);
                            costcenters.Add(costcenter);
                        }
                    }
                }
            }

            return costcenters;
        }


        public List<CostCenter> GetWithChildren(int deptsap)
        {
            List<CostCenter> costcenters = new List<CostCenter>();

            using (SqlConnection conn = new SqlConnection(SqlManager.ConnectionString))
            {
                conn.Open();
                string cmd_get_with_children = @"
                    --DECLARE @DeptSap int
                    --SET @DeptSap = 3108, 3180, 2946 

                    ;WITH DeptTree as(
                        SELECT * FROM V_department
                        WHERE dept_sap = @DeptSap

                        UNION ALL

                        SELECT d.* FROM V_department d
                        INNER JOIN DeptTree x ON d.dept_upper= x.dept_sap
                    )
                    SELECT * 
                    FROM CostCenter 
                    WHERE CostCenterID IN 
	                    (
		                    SELECT cost_center_code FROM DeptTree 
	                    )
                    AND [Status] = 1
                    ORDER BY CostCenterID
                ";

                using (SqlCommand cmd = new SqlCommand(cmd_get_with_children, conn))
                {

                    cmd.Parameters.AddWithValue("DeptSap", deptsap);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CostCenter costcenter = ConvertReaderToCostCenter(reader);
                            costcenters.Add(costcenter);
                        }
                    }
                }
            }

            return costcenters;
        }
        #endregion


        #region Add 

        public CostCenter Add(string id, string name, string dept_name)
        {
            CostCenter costcenter = new CostCenter();
            costcenter.CostCenterID = id;
            costcenter.CostCenterName = name;
            costcenter.ShortName = dept_name;
            costcenter.Status = Models.Base.RecordStatus.Active;
            costcenter.NewCreateTimeStamp();
            _db.CostCenters.Add(costcenter);
            _db.SaveChanges();
            return costcenter;
        }

        #endregion


        #region Methods

        public CostCenter ConvertReaderToCostCenter(SqlDataReader reader)
        {
            CostCenter costcenter = new CostCenter();
            costcenter.CostCenterID = reader["CostCenterID"].ToString();
            costcenter.CostCenterName = reader["CostCenterName"].ToString();
            costcenter.ShortName = reader["ShortName"].ToString();
            costcenter.LongName = reader["LongName"].ToString();
            costcenter.Status = RecordStatus.Active;
            return costcenter;
        }

        #endregion

    }
}