using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Repository
{
    // Specific repository implementation: Entity Framework
    public class EntityFrameworkReadOnlyRepository<TContext> : IReadOnlyRepository
        where TContext : DbContext
    {
        protected readonly TContext Context;

        public EntityFrameworkReadOnlyRepository(TContext context)
        {
            this.Context = context;
        }

        public bool IsConnected()
        {
            try
            {
                if (Context.Database.Connection.State == System.Data.ConnectionState.Open){
                    return true;
                }
                Context.Database.Connection.Open();
                return Context.Database.Connection.State == System.Data.ConnectionState.Open;
            } catch (Exception)
            {
                return false;
            }
        }

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>  orderBy = null,
            params Expression<Func<TEntity, object>>[] nagiationProperties 
        )
            where TEntity : class
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            // Assign where claus
            if (filter != null)
                query = query.Where(filter);

            //load includes
            foreach (var includeProperty in nagiationProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] nagiationProperties
        ) 
            where TEntity : class
        {
            return GetQueryable<TEntity>(null, orderBy, nagiationProperties).ToList();
        }

        public IEnumerable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] nagiationProperties
        ) 
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter, orderBy, nagiationProperties).ToList();
        }

        public TEntity GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] nagiationProperties
        ) 
            where TEntity : class
        {
            // Returns the entity or default (is NULL)
            return GetQueryable<TEntity>(filter, null, nagiationProperties).SingleOrDefault();
        }
    }
}
