using BudgetControl.IdmEmployeeServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BudgetControl.Manager
{
    public class IdmManager
    {
        //private IdmServicesSoapClient _idmService;
        private EmployeeServicesSoapClient _empService;

        // PEA Web API Key
        private const string _idmWsAuthenKey = "e3358fc1-99ad-4b21-8237-7c9c8ba1c5dc";
        private const string _empWsAuthenKey = "93567815-dfbb-4727-b4da-ce42c046bfca";
        #region Constructor

        public IdmManager()
        {

        }

        #endregion


        #region Get Employee

        public EmployeeProfile GetEmployeeProfile(string empno)
        {

            try
            {
                _empService = new EmployeeServicesSoapClient();
                EmployeeProfile emp;
                ServiceRequestOfEmployeeInfoByEmployeeIdCriteria empInfoCriteria = new ServiceRequestOfEmployeeInfoByEmployeeIdCriteria()
                {
                    InputObject = new EmployeeInfoByEmployeeIdCriteria()
                    {
                        EmployeeId = empno
                    },
                    WSAuthenKey = _empWsAuthenKey
                };
                emp = _empService.GetEmployeeInfoByEmployeeId(empInfoCriteria).ResultObject;

                return emp;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_empService != null)
                {
                    _empService.Close();
                }
            }
        }

        #endregion
    }
}