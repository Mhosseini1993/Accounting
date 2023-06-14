using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.EF_Model;

namespace Accounting.DataLayer.GenericRepository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private Accounting_DBEntities _connection;
        private DbSet<TEntity> _tableName;

        public GenericRepository(Accounting_DBEntities db)
        {
            this._connection = db;
            _tableName = _connection.Set<TEntity>();// cmake connection between table and entity
        }
        public virtual TEntity GetByID(object ID)
        {
            return _tableName.Find(ID);
        }
        public virtual void Insert(TEntity entity)
        {
            _tableName.Add(entity);
        }
        public virtual void Delete(TEntity entity)
        {
            if (_connection.Entry(entity).State == EntityState.Detached)
                _tableName.Attach(entity);
            //_tableName.Remove(entity);
            _connection.Entry(entity).State = EntityState.Deleted;
        }
        public virtual void Delete(object ID)
        {
            TEntity entity = GetByID(ID);
            Delete(entity);
        }
        public virtual void Update(TEntity entity)
        {
            if (_connection.Entry(entity).State == EntityState.Detached)
                _tableName.Attach(entity);
            _connection.Entry(entity).State = EntityState.Modified;
        }
        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity,bool>> where=null)
        {
            IQueryable<TEntity> query = _tableName;
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.ToList();
        }
    }
}
