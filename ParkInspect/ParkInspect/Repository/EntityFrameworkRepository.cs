using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Repository
{
    public class EntityFrameworkRepository<TContext> : EntityFrameworkReadOnlyRepository<TContext>, IRepository
        where TContext : DbContext
    {
        public EntityFrameworkRepository(TContext context) : base(context)
        {
        }

        public void Create<TEntity>(TEntity entity, string createdBy = null) 
            where TEntity : class
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity, string modifiedBy = null) where TEntity : class
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<TEntity>(object id) where TEntity : class
        {
            TEntity entity = Context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            var dbSet = Context.Set<TEntity>();
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }
        }

        // Insert or update pattern
        // https://msdn.microsoft.com/en-us/data/jj592676.aspx
        public bool InsertOrUpdate<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Modified)
                Update(entity);
            else if (entry.State == EntityState.Detached)
                Create(entity);
            else
                return false;

            return true;
        }

        protected virtual void ThrowEnhancedValidationException(DbEntityValidationException e)
        {
            var errorMesssage = e.EntityValidationErrors
                .SelectMany(x => x.ValidationErrors)
                .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("; ", errorMesssage);
            var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);
            throw new DbEntityValidationException(exceptionMessage, e.EntityValidationErrors);
        }
    }
}
