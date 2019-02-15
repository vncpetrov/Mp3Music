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
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public async Task CalculateExecutionTimeOfDecoratedQuery()
        {
            var performanceRepositoryMock = new Mock<IEfRepository<PerformanceEntry>>();
            TimeSpan executionTime = default(TimeSpan);

            performanceRepositoryMock.Setup(x => x.Add(It.IsAny<PerformanceEntry>()))
                .Callback<PerformanceEntry>(entry => executionTime = entry.Duration);
            
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var timeProviderStub = new Mock<IDateTimeProvider>();

            QueryServiceStub decorateeStub = new QueryServiceStub();

            // Arrange
            PerformanceQueryServiceDecorator<QueryStub, object> sut = new PerformanceQueryServiceDecorator<QueryStub, object>(
                performanceRepository: performanceRepositoryMock.Object,
                contextSaveChanges: contextSaveChangesStub.Object,
                timeProvider: timeProviderStub.Object,
                decoratee: decorateeStub);

            // Act
            await sut.ExecuteAsync(new QueryStub());

            // Assert
            Assert.That(executionTime > TimeSpan.MinValue);
        }

        [Test]
        public async Task CallThePassedDecoratee()
        {
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>(); 

            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var timeProviderStub = new Mock<IDateTimeProvider>();

            var deorateeMock = new Mock<IQueryService<QueryStub, object>>();

            // Arrange
            PerformanceQueryServiceDecorator<QueryStub, object> sut = new PerformanceQueryServiceDecorator<QueryStub, object>(
                performanceRepository: performanceRepositoryStub.Object,
                contextSaveChanges: contextSaveChangesStub.Object,
                timeProvider: timeProviderStub.Object,
                decoratee: deorateeMock.Object);

            // Act
            await sut.ExecuteAsync(new QueryStub());
             
            // Assert
            deorateeMock.Verify(
                x => x.ExecuteAsync(It.IsAny<QueryStub>()), Times.Once);
        }

        [Test]
        public async Task AppendEntryToPerformanceRepository()
        {
            var performanceRepositoryMock = new Mock<IEfRepository<PerformanceEntry>>();

            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var timeProviderStub = new Mock<IDateTimeProvider>();

            var deorateeStub = new Mock<IQueryService<QueryStub, object>>();

            // Arrange
            PerformanceQueryServiceDecorator<QueryStub, object> sut = new PerformanceQueryServiceDecorator<QueryStub, object>(
                performanceRepository: performanceRepositoryMock.Object,
                contextSaveChanges: contextSaveChangesStub.Object,
                timeProvider: timeProviderStub.Object,
                decoratee: deorateeStub.Object);

            // Act
            await sut.ExecuteAsync(new QueryStub());

            // Assert
            performanceRepositoryMock.Verify(
                x => x.Add(It.IsAny<PerformanceEntry>()), Times.Once);
        }

        [Test]
        public async Task SaveChangesMadeToPerformanceRepository()
        {
            var performanceRepositoryStub = new Mock<IEfRepository<PerformanceEntry>>();

            var contextSaveChangesMock = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            var timeProviderStub = new Mock<IDateTimeProvider>();

            var deorateeStub = new Mock<IQueryService<QueryStub, object>>();

            // Arrange
            PerformanceQueryServiceDecorator<QueryStub, object> sut = new PerformanceQueryServiceDecorator<QueryStub, object>(
                performanceRepository: performanceRepositoryStub.Object,
                contextSaveChanges: contextSaveChangesMock.Object,
                timeProvider: timeProviderStub.Object,
                decoratee: deorateeStub.Object);

            // Act
            await sut.ExecuteAsync(new QueryStub());

            // Assert
            contextSaveChangesMock.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
