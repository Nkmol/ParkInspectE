using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using ParkInspect.Repository;

namespace ParkInspect.Test
{
    public class MockRepositoryWrapper<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext, new()
    {
        private readonly List<TEntity> _data;

        public MockRepositoryWrapper()
        {
            _data = new List<TEntity>();
            Repo = new EntityFrameworkRepository<TContext>(CreateContextMock());
        }

        public Mock<DbSet<TEntity>> MockEntities { get; private set; }
        public EntityFrameworkRepository<TContext> Repo { get; }

        private TContext CreateContextMock()
        {
            var queryable = _data.AsQueryable();
            MockEntities = new Mock<DbSet<TEntity>>();
            MockEntities.As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(queryable.Provider);
            MockEntities.As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(queryable.Expression);
            MockEntities.As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(queryable.ElementType);
            MockEntities.As<IQueryable<TEntity>>()
                .Setup(x => x.GetEnumerator())
                .Returns(() => queryable.GetEnumerator());
            MockEntities.Setup(x => x.Add(It.IsAny<TEntity>())).Callback<TEntity>(x => _data.Add(x));
            MockEntities.Setup(x => x.Remove(It.IsAny<TEntity>())).Callback<TEntity>(x => _data.Remove(x));

            var mockContext = new Mock<TContext>();
            mockContext.Setup(x => x.Set<TEntity>()).Returns(MockEntities.Object);

            return mockContext.Object;
        }

        public void ClearData()
        {
            _data.Clear();
        }
    }
}