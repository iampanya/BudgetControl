using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

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

        #region Create

        public void Add(Account account)
        {
            // 1. Validate entity
            this.Validate(account);

            // 2. Create TimeStamp 
            account.NewCreateTimeStamp();

            // 2. Add to database
            _accountRepo.Add(account);
            _accountRepo.Save();
        }

        #endregion

        #region Retrieve

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

        #region Update

        public void Update(Account account)
        {
            // 1. Validate entity
            this.Validate(account);

            // 1. Modify TimeStame
            account.NewCreateTimeStamp();

            // 2. Update to database
            _accountRepo.Update(account);
            _accountRepo.Save();
        }

        #endregion

        #region Deactivate

        public void Deactivate(string accountid)
        {
            this.Deactivate(this.GetByID(accountid));
        }

        public void Deactivate(Account account)
        {
            //1. Validate entity
            this.Validate(account);

            account.Status = RecordStatus.Inactive;
            this.Update(account);
        }

        #endregion

        #region Delete

        public void Delete(string accountid)
        {
            this.Delete(this.GetByID(accountid));
        }

        public void Delete(Account account)
        {
            //1. Validate entity
            this.Validate(account);

            // 2. Modify timestamp
            account.NewModifyTimeStamp();

            // 3. Delete from datatbase
            _accountRepo.Delete(account);
            _accountRepo.Save();
        }

        #endregion

        #region Methods

        public void Validate(Account account)
        {
            // 1. Check entity is null
            if(account == null)
            {
                throw new Exception("Account cannot be null");
            }

            // 2. Check content inside entity
            var validateResult = account.Validate();
            if (!validateResult.IsSuccess)
            {
                throw new Exception(validateResult.ErrorMessage);
            }
        }

        #endregion

    }
}