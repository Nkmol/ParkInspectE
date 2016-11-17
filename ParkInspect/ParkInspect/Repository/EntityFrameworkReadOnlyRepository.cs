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

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>  orderBy = null,
            string includeproperties = null,
            int? skip = null,
            int? take = null
        )
            where TEntity : class
        {
            includeproperties = includeproperties ?? string.Empty; // Needs string format
            IQueryable<TEntity> query = Context.Set<TEntity>();

            // Assign where claus
            if (filter != null)
                query = query.Where(filter);

            //load includes
            foreach (
                var includeProperty in includeproperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query;
        }

        public IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProperties = null, 
            int? skip = null, 
            int? take = null
        ) 
            where TEntity : class
        {
            return GetQueryable<TEntity>(null, orderBy, includeProperties, skip, take).ToList();
        }

        public IEnumerable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProperties = null,
            int? skip = null, 
            int? take = null
        ) 
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter, orderBy, includeProperties, skip, take).ToList();
        }

        public TEntity GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter = null, 
            string includeProperties = null
        ) 
            where TEntity : class
        {
            // Returns the entity or default (is NULL)
            return GetQueryable<TEntity>(filter, null, includeProperties).SingleOrDefault();
        }
    }
}
