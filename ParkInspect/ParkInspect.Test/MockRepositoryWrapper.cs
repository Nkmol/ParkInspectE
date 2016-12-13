using Moq;
using ParkInspect;
using ParkInspect.Repository;
using ParkInspect.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class MockRepositoryWrapper<TEntity, TContext>
        where TEntity : class 
        where TContext : DbContext
    {
        private List<TEntity> _data;
        public Mock<DbSet<TEntity>> MockEntities { get; private set; }
        public EntityFrameworkRepository<TContext> Repo { get; }
        public MockRepositoryWrapper()
        {
            _data = new List<TEntity>();
            Repo = new EntityFrameworkRepository<TContext>(CreateContextMock());
        }

        private TContext CreateContextMock()
        {
            var queryable = _data.AsQueryable();
            MockEntities = new Mock<DbSet<TEntity>>();
            MockEntities.As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(queryable.Provider);
            MockEntities.As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(queryable.Expression);
            MockEntities.As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(queryable.ElementType);
            MockEntities.As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            MockEntities.Setup(x => x.Add(It.IsAny<TEntity>())).Callback<TEntity>(x => _data.Add(x));
            MockEntities.Setup(x => x.Remove(It.IsAny<TEntity>())).Callback<TEntity>(x => _data.Remove(x));
            // TODO Update
            //MockEntities.Setup(x => x.Attach(It.IsAny<TEntity>())).Callback<TEntity>(x => _date.get);

            var MockContext = new Mock<TContext>();
            MockContext.Setup(x => x.Set<TEntity>()).Returns(MockEntities.Object);

            return MockContext.Object;
        }

        public void ClearData()
        {
            _data.Clear();
        }
    }
}
