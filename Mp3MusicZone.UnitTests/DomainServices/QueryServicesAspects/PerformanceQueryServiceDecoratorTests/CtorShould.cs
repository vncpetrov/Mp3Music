namespace Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.
    PerformanceQueryServiceDecoratorTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServicesAspects;
    using Mp3MusicZone.EfDataAccess;
    using Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedPerformanceRepositoryIsNull()
        {
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] {"Fake Connection string"});
            var timeProviderStub = new Mock<IDateTimeProvider>();
            var decorateeStub = new QueryServiceStub();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new PerformanceQueryServiceDecorator<QueryStub, object>(
                    performanceRepository: null,
                    contextSaveChanges: contextSaveChangesStub.Object,
                    timeProvider: timeProviderStub.Object,
                    decoratee: decorateeStub));
        }

        [Test]
        public void SavePassedPerformanceRepositoryWhenIsNotNull()
        {
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });
            var timeProviderStub = new Mock<IDateTimeProvider>();

            QueryServiceStub decorateeStub = new QueryServiceStub();
             
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>();

            // Arrange && Act
            PerformanceQueryServiceDecorator<QueryStub, object> sut = new PerformanceQueryServiceDecorator<QueryStub, object>(
                    performanceRepository: performanceRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object,
                    timeProvider: timeProviderStub.Object,
                    decoratee: decorateeStub);

            // Assert
            var actualPerformanceRepository = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<PerformanceEntry>))
                .GetValue(sut);

            Assert.AreSame(performanceRepositoryStub.Object, actualPerformanceRepository);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedContextSaveChangesIsNull()
        {
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>(); 

            var timeProviderStub = new Mock<IDateTimeProvider>();

            var decorateeStub = new QueryServiceStub();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new PerformanceQueryServiceDecorator<QueryStub, object>(
                    performanceRepository: performanceRepositoryStub.Object,
                    contextSaveChanges: null,
                    timeProvider: timeProviderStub.Object,
                    decoratee: decorateeStub));
        }

        [Test]
        public void SavePassedContextSaveChangesWhenIsNotNull()
        {
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>();

            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var timeProviderStub = new Mock<IDateTimeProvider>();

            QueryServiceStub decorateeStub = new QueryServiceStub();
             
            // Arrange && Act
            PerformanceQueryServiceDecorator<QueryStub, object> sut = new PerformanceQueryServiceDecorator<QueryStub, object>(
                    performanceRepository: performanceRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object,
                    timeProvider: timeProviderStub.Object,
                    decoratee: decorateeStub);

            // Assert
            var actualContextSaveChanges = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfDbContextSaveChanges))
                .GetValue(sut);

            Assert.AreSame(contextSaveChangesStub.Object, actualContextSaveChanges);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedTimeProviderIsNull()
        {
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>();
            
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var decorateeStub = new QueryServiceStub();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new PerformanceQueryServiceDecorator<QueryStub, object>(
                    performanceRepository: performanceRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object,
                    timeProvider: null,
                    decoratee: decorateeStub));
        }

        [Test]
        public void SavePassedTimeProviderWhenIsNotNull()
        {
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>();

            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var timeProviderStub = new Mock<IDateTimeProvider>();

            QueryServiceStub decorateeStub = new QueryServiceStub();

            // Arrange && Act
            PerformanceQueryServiceDecorator<QueryStub, object> sut = new PerformanceQueryServiceDecorator<QueryStub, object>(
                    performanceRepository: performanceRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object,
                    timeProvider: timeProviderStub.Object,
                    decoratee: decorateeStub);

            // Assert
            var actualTimeProvider = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IDateTimeProvider))
                .GetValue(sut);

            Assert.AreSame(timeProviderStub.Object, actualTimeProvider);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedDecorateeIsNull()
        {
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>();

            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var timeProviderStub = new Mock<IDateTimeProvider>();
              
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new PerformanceQueryServiceDecorator<QueryStub, object>(
                    performanceRepository: performanceRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object,
                    timeProvider: timeProviderStub.Object,
                    decoratee: null));
        }

        [Test]
        public void SavePassedDecorateeWhenIsNotNull()
        {
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>();

            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var timeProviderStub = new Mock<IDateTimeProvider>();

            QueryServiceStub decorateeStub = new QueryServiceStub();

            // Arrange && Act
            PerformanceQueryServiceDecorator<QueryStub, object> sut = new PerformanceQueryServiceDecorator<QueryStub, object>(
                    performanceRepository: performanceRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object,
                    timeProvider: timeProviderStub.Object,
                    decoratee: decorateeStub);

            // Assert
            var actualDecoratee = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IQueryService<QueryStub, object>))
                .GetValue(sut);

            Assert.AreSame(decorateeStub, actualDecoratee);
        }
    }
}
