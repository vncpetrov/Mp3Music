namespace Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.
    Caching.CacheQueryServiceProxyTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServicesAspects.Caching;
    using Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public async Task CacheTheQueryResultIfIsNotInTheCache()
        {
            var queryServiceStub = new QueryServiceStub();

            var cacheManagerStub = new Mock<ICacheManager>();

            cacheManagerStub.Setup(cm => cm.Exists(It.IsAny<string>()))
                .Returns(false);

            var optionsStub = new CacheOptions(false, int.MaxValue); 
            var userContextStub = new Mock<IUserContext>();

            // Arrange 
            CacheQueryServiceProxy<QueryStub, object> sut =
                new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: userContextStub.Object);

            // Act
            await sut.ExecuteAsync(new QueryStub());

            // Assert
            cacheManagerStub.Verify(cm => cm.Add(
                It.IsAny<string>(), It.IsAny<object>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task ReturnQueryResultFromCacheIfExistsInTheCache()
        {
            string expectedResult = "Returned from cache";

            var queryServiceStub = new QueryServiceStub();

            var cacheManagerStub = new Mock<ICacheManager>();

            cacheManagerStub.Setup(cm => cm.Exists(It.IsAny<string>()))
                .Returns(true);

            cacheManagerStub.Setup(cm => cm.Get<object>(It.IsAny<string>()))
                .Returns(expectedResult);

            var optionsStub = new CacheOptions(false, int.MaxValue);
            var userContextStub = new Mock<IUserContext>();

            // Arrange 
            CacheQueryServiceProxy<QueryStub, object> sut =
                new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: userContextStub.Object);

            // Act
            string actualResult = (string)await sut.ExecuteAsync(new QueryStub());

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task NotCallThePassedQueryServiceWhenItsResultIsCached()
        {
            var queryServiceStub = new Mock<IQueryService<QueryStub, object>>();

            var cacheManagerStub = new Mock<ICacheManager>();

            cacheManagerStub.Setup(cm => cm.Exists(It.IsAny<string>()))
                .Returns(true);

            var optionsStub = new CacheOptions(false, int.MaxValue);
            var userContextStub = new Mock<IUserContext>();

            // Arrange 
            CacheQueryServiceProxy<QueryStub, object> sut =
                new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub.Object,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: userContextStub.Object);

            // Act
            string actualResult = (string)await sut.ExecuteAsync(new QueryStub());

            // Assert
            queryServiceStub.Verify(
                q => q.ExecuteAsync(It.IsAny<QueryStub>()), Times.Never);
        }
    }
}
