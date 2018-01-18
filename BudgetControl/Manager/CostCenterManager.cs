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
                    DECLARE @DeptCode varchar(15)
                    SELECT TOP(1) @DeptCode = DeptChangeCode
                    FROM DepartmentInfo 
                    WHERE CostCenterCode = @CostCenterCode

                    SELECT * 
                    FROM CostCenter
                    WHERE CostCenterID IN ( 
	                    SELECT CostCenterCode
	                    FROM DepartmentInfo 
	                    WHERE DeptChangeCode LIKE REPLACE(RTRIM(REPLACE(@DeptCode,'0',' ')),' ','0') + '%'
                    ) AND Status = 1
                ";

                using (SqlCommand cmd = new SqlCommand(cmd_get_with_children, conn))
                {
                    cmd.Parameters.AddWithValue("CostCenterCode", costcenterid);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CostCenter costcenter = new CostCenter();
                            costcenter.CostCenterID = reader["CostCenterID"].ToString();
                            costcenter.CostCenterName = reader["CostCenterName"].ToString();
                            costcenter.ShortName = reader["ShortName"].ToString();
                            costcenter.LongName = reader["LongName"].ToString();
                            costcenter.Status = RecordStatus.Active;
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

    }
}