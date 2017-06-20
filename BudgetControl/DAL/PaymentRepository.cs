using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BudgetControl.Models.Base;

namespace BudgetControl.DAL
{
    public class PaymentRepository : IRepository<Payment>, IDisposable
    {
        private BudgetContext _db;

        #region Constructor
        public PaymentRepository()
        {
            _db = new BudgetContext();
            InitInstance();
        }

        public PaymentRepository(BudgetContext context)
        {
            this._db = context;
            InitInstance();
        }


        private void InitInstance()
        {

        }

        #endregion

        #region IRepository

        public IEnumerable<Payment> GetAll()
        {
            return _db.Payments
                .AsNoTracking()
                .Include(p => p.Controller)
                .Include(p => p.Requester)
                .Include(p => p.Statements)
                .Include(p => p.BudgetTransactions)
                .Include(p => p.CostCenter);
        }

        public IEnumerable<Payment> Get()
        {
            //TODO filter from CostCenter
            return GetAll();
            /*return GetAll().Where(p => p.Status == RecordStatus.Active)*/;
        }

        public void Add(Payment entity)
        {
            // 1. Validate entity
            entity.Validate();

            // 2. Mark timestamp
            entity.NewCreateTimeStamp();

            // 3. Add to context
            _db.Payments.Add(entity);

        }
        
        public void AddOrUpdate(Payment entity)
        {
            if (GetById(entity.PaymentID) == null)
            {
                Add(entity);
            }
            else
            {
                Update(entity);
            }
        }

        public void Delete(object id)
        {
            this.Delete(this.GetById(id));
        }

        public void Delete(Payment entity)
        {
            entity.Status = RecordStatus.Inactive;
            Update(entity);
        }

        public void Update(Payment entity)
        {
            // 1. Validate
            entity.Validate();

            // 2. Mark timestamp
            entity.NewModifyTimeStamp();

            // 3. Add to context
            _db.Entry(entity).State = EntityState.Modified;
            _db.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            _db.Entry(entity).Property(x => x.CreatedBy).IsModified = false;

        }

        public Payment GetById(object id)
        {
            Guid paymentid;
            try
            {
                paymentid = new Guid(id.ToString());
            }
            catch (Exception)
            {
                return null;
            }
            return GetAll().FirstOrDefault(p => p.PaymentID == paymentid);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        #endregion

        #region Implement Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        
        #region Additional Method

        public int GetPaymentSequence(string year)
        {
            
            try
            {
                int max_sequence = 0;
                using (BudgetContext db = new BudgetContext())
                {
                    var budget = db.Budgets.Where(b => b.Year == year.Trim()).FirstOrDefault();
                    if (budget == null)
                    {
                        throw new Exception("Invalid Year");
                    }
                    var last_payment = db.Payments
                        .Where(p => p.Year == year)
                        .OrderByDescending(p => p.Sequence)
                        .FirstOrDefault();
                    if (last_payment != null)
                    {
                        max_sequence = last_payment.Sequence;
                    }
                    return max_sequence + 1;
                }
            }
            catch (Exception)
            {
                throw new Exception("ERROR: Can not get Payment Sequence.");
            }
        }

        #endregion




    }
}