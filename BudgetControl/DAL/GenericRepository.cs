using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetControl.DAL;
using System.Data.Entity;

namespace BudgetControl.DAL
{
    public class GenericRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        private BudgetContext _db;
        private DbSet<TEntity> _table;

        #region Constructor

        public GenericRepository()
        {
            this._db = new BudgetContext();
            this._table = _db.Set<TEntity>();
        }

        public GenericRepository(BudgetContext context)
        {
            this._db = context;
            this._table = _db.Set<TEntity>();
        }

        #endregion

        #region IRepository

        public void Add(TEntity entity)
        {
            this._table.Add(entity);
        }

        public void AddOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = _table.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            if(_db.Entry(entity).State == EntityState.Detached)
            {
                _table.Attach(entity);
            }
            _table.Remove(entity);
        }     

        public IEnumerable<TEntity> Get()
        {
            return _table;
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(object id)
        {
            return _table.Find(id);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
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
    }
}