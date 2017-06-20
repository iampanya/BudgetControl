using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class AccountManager
    {
        private BudgetContext _db;
        private GenericRepository<Account> _accountRepo;

        #region Constructor

        public AccountManager()
        {
            this._db = new BudgetContext();
            this._accountRepo = new GenericRepository<Account>(this._db);
        }

        public AccountManager(BudgetContext context)
        {
            this._db = context;
            this._accountRepo = new GenericRepository<Account>(this._db);
        }

        #endregion

        #region GetAccount

        public IEnumerable<Account> GetAll()
        {
            return _accountRepo.Get();
        }

        private IEnumerable<Account> GetByStatus(RecordStatus status)
        {
            return GetAll().Where(a => a.Status == status);
        }

        public IEnumerable<Account> GetActiveBudget()
        {
            return GetByStatus(RecordStatus.Active);
        }

        public IEnumerable<Account> GetRemoveBudget()
        {
            return GetByStatus(RecordStatus.Inactive);
        }

        public Account GetByID(string accountid)
        {
            return _accountRepo.GetById(accountid);
        }

        #endregion


        #region Add new Account

        public void Add(Account account)
        {
            // 1. Create TimeStamp 
            account.NewCreateTimeStamp();

            // 2. Add to database
            _accountRepo.Add(account);
            _accountRepo.Save();
        }

        #endregion

        #region Update

        public void Update(Account account)
        {
            // 1. Modify TimeStame
            account.NewCreateTimeStamp();

            // 2. Update to database
            _accountRepo.Update(account);
            _accountRepo.Save();
        }

        #endregion

        #region 

        #endregion
    }
}