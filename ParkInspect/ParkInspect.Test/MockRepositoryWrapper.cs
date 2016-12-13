using Moq;
using ParkInspect;
using ParkInspect.Repository;
using ParkInspect.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class MockRepositoryWrapper<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext, new()
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

            // Update: Need to know by which PK it will determ if it is the SAME entity
            // Get primary keys by Entity type
            var PKs = GetPrimaryKeyPropertyNames().ToList();
            // Update by remove + add (old determed by PKs)
            MockEntities.Setup(x => x.Attach(It.IsAny<TEntity>())).Callback<TEntity>(x =>
            {
                // Determ if all PKs with value exists in _data
                var same = 0;
                var candidates = _data;
                while (PKs.Count() != same) {
                    var name = PKs[same];
                    candidates = candidates.Where(d => typeof(TEntity).GetProperty(name).GetValue(d) == typeof(TEntity).GetProperty(name).GetValue(x)).ToList();
                    if (candidates.Count() > 0)
                    {
                        same++;

                        // If we have determed that our entity exists(Checked on all PK values) -> Remove + add = update
                        if (PKs.Count() == same)
                        {
                            _data.RemoveAll(e => candidates.Contains(e));
                        }
                    }
                    else
                        break;
                }
            });

            var MockContext = new Mock<TContext>();
            MockContext.Setup(x => x.Set<TEntity>()).Returns(MockEntities.Object);

            return MockContext.Object;
        }

        private IEnumerable<string> GetPrimaryKeyPropertyNames()
        {
            var objectContext = (new TContext() as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
            var type = typeof(TEntity);

            if (objectContext.MetadataWorkspace.TryGetType(type.Name, type.Namespace, DataSpace.OSpace, out var edmType))
            {
                return edmType.MetadataProperties.Where(x => x.Name == "KeyMembers")
                    .SelectMany(x => x.Value as ReadOnlyMetadataCollection<EdmMember>)
                    .OfType<EdmProperty>().Select(property => property.Name);
            }

            return Enumerable.Empty<string>();
        }

        public void ClearData()
        {
            _data.Clear();
        }
    }
}
