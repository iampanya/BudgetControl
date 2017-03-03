using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BudgetControl.Models.Base;
using BudgetControl.Sessions;

namespace BudgetControl.DAL
{
    public class PaymentRepository : IRepository<Payment>, IDisposable
    {
        private BudgetContext context;
        private CostCenter _workingCostCenter;

        #region Constructor
        public PaymentRepository()
        {
            context = new BudgetContext();
            InitInstance();
        }

        public PaymentRepository(BudgetContext context)
        {
            this.context = context;
            InitInstance();
        }


        private void InitInstance()
        {
            this._workingCostCenter = AuthManager.GetWorkingCostCenter();
            if (this._workingCostCenter == null)
            {
                throw new Exception("Please Login");
            }
        }

        #endregion
        public IEnumerable<Payment> Get()
        {
            //TODO filter from CostCenter
            return context.Payments
                .Include(p => p.Controller)
                .Include(p => p.Requester)
                .Include(p => p.Statements)
                .Include(p => p.CostCenter)
                .AsNoTracking()
                .Where(p => p.CostCenterID.StartsWith(this._workingCostCenter.CostCenterTrim) && p.Status == RecordStatus.Active)
                .OrderBy(p => p.Sequence);
        }

        public void Add(Payment entity)
        {
            //TODO Insert statements 
            if (entity == null)
            {
                //TODO set exception
                throw new Exception();
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    RecordTimeStamp timestamp = new RecordTimeStamp();
                    timestamp.NewCreateTimeStamp();
                    List<Statement> statements = new List<Statement>();

                    //Initial Payment
                    //entity.PaymentID = Guid.NewGuid();
                    entity.Sequence = GetPaymentSequence(entity.Year);

                    // Assign value in Backend layer
                    entity.PaymentDate = DateTime.Today;
                    entity.ControlBy = AuthManager.GetCurrentUser().EmployeeID; //TODO check controller

                    entity.Status = RecordStatus.Active;
                    entity.SetCreateTimeStamp(timestamp);
                    context.Payments.Add(entity);

                    //Initial Statements
                    statements = entity.Statements.ToList();
                    statements.ForEach(s =>
                    {
                        s.StatementID = Guid.NewGuid();
                        s.PaymentID = entity.PaymentID;
                        s.Status = RecordStatus.Active;
                        s.SetCreateTimeStamp(timestamp);
                        context.Statements.Add(s);
                    });

                    context.SaveChanges();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

        public void Delete(object id)
        {
            Guid paymentid = new Guid(id.ToString());
            var entity = context.Payments.Where(p => p.PaymentID == paymentid).FirstOrDefault();
            if (entity == null)
            {
                throw new Exception("Entity not found.");
            }
            RecordTimeStamp rt = new RecordTimeStamp();
            rt.NewCreateTimeStamp();
            entity.Status = RecordStatus.Remove;
            entity.SetModifiedTimeStamp(rt);
            context.Payments.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(Payment entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Payment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    RecordTimeStamp timestamp = new RecordTimeStamp();
                    timestamp.NewCreateTimeStamp();

                    List<Statement> statements = new List<Statement>();
                    statements = entity.Statements.ToList();
                    entity.Statements = new List<Statement>();
                    context.Payments.Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;
                    context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                    context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    context.Entry(entity).Property(x => x.Sequence).IsModified = false;
                    context.Entry(entity).Property(x => x.PaymentDate).IsModified = false;

                    //List<Statement> statements = new List<Statement>();
                    //statements = entity.Statements.ToList();
                    statements.ForEach(s =>
                    {
                        if (s.StatementID == Guid.Empty)
                        {
                            s.StatementID = Guid.NewGuid();
                            s.PaymentID = entity.PaymentID;
                            s.Status = RecordStatus.Active;
                            s.SetCreateTimeStamp(timestamp);
                            context.Statements.Add(s);
                        }
                        else
                        {
                            s.SetModifiedTimeStamp(timestamp);
                            context.Statements.Attach(s);
                            context.Entry(s).State = EntityState.Modified;
                            context.Entry(s).Property(x => x.CreatedAt).IsModified = false;
                            context.Entry(s).Property(x => x.CreatedBy).IsModified = false;
                        }
                    });

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
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
                throw new Exception("Payment ID is invalid");
            }
            return Get().Where(p => p.PaymentID == paymentid).FirstOrDefault();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        #region Implement Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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


        public void AddOrUpdate(Payment entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Payment> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}