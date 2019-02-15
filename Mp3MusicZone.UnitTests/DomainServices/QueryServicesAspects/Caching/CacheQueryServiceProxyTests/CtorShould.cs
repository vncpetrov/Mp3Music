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
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedQueryServiceIsNull()
        {
            var cacheManagerStub = new Mock<ICacheManager>();
            var userContextStub = new Mock<IUserContext>();
            var optionsStub = new CacheOptions(false, int.MaxValue);

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: null,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: userContextStub.Object));
        }

        [Test]
        public void SavePassedQueryServiceWhenIsNotNull()
        {
            var cacheManagerStub = new Mock<ICacheManager>();
            var userContextStub = new Mock<IUserContext>();
            var optionsStub = new CacheOptions(false, int.MaxValue);
            var queryServiceStub = new QueryServiceStub();

            // Arrange 
            CacheQueryServiceProxy<QueryStub, object> sut =
                new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: userContextStub.Object);

            // Assert
            var actualQueryService = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IQueryService<QueryStub, object>))
                .GetValue(sut);

            Assert.AreSame(queryServiceStub, actualQueryService);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedCacheManagerIsNull()
        {
            var queryServiceStub = new QueryServiceStub();
            var userContextStub = new Mock<IUserContext>();
            var optionsStub = new CacheOptions(false, int.MaxValue);
             
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: null,
                    options: optionsStub,
                    userContext: userContextStub.Object));
        }

        [Test]
        public void SavePassedCacheManagerWhenIsNotNull()
        {
            var cacheManagerStub = new Mock<ICacheManager>();
            var userContextStub = new Mock<IUserContext>();
            var optionsStub = new CacheOptions(false, int.MaxValue);
            var queryServiceStub = new QueryServiceStub();

            // Arrange 
            CacheQueryServiceProxy<QueryStub, object> sut =
                new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: userContextStub.Object);

            // Assert
            var actualCacheManager = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(ICacheManager))
                .GetValue(sut);

            Assert.AreSame(cacheManagerStub.Object, actualCacheManager);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedUserContextIsNull()
        {
            var queryServiceStub = new QueryServiceStub();
            var cacheManagerStub = new Mock<ICacheManager>();
            var optionsStub = new CacheOptions(false, int.MaxValue);

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: null));
        }

        [Test]
        public void SavePassedUserContextWhenIsNotNull()
        {
            var queryServiceStub = new QueryServiceStub();
            var cacheManagerStub = new Mock<ICacheManager>();
            var optionsStub = new CacheOptions(false, int.MaxValue);
            var userContextStub = new Mock<IUserContext>();

            // Arrange 
            CacheQueryServiceProxy<QueryStub, object> sut =
                new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: userContextStub.Object);

            // Assert
            var actualUserContext = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IUserContext))
                .GetValue(sut);

            Assert.AreSame(userContextStub.Object, actualUserContext);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedOptionsIsNull()
        {
            var queryServiceStub = new QueryServiceStub();
            var cacheManagerStub = new Mock<ICacheManager>();
            var userContextStub = new Mock<IUserContext>();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: cacheManagerStub.Object,
                    options: null,
                    userContext: userContextStub.Object));
        }

        [Test]
        public void SavePassedOptionsWhenIsNotNull()
        {
            var queryServiceStub = new QueryServiceStub();
            var cacheManagerStub = new Mock<ICacheManager>();
            var optionsStub = new CacheOptions(false, int.MaxValue);

            var userContextStub = new Mock<IUserContext>();

            // Arrange 
            CacheQueryServiceProxy<QueryStub, object> sut =
                new CacheQueryServiceProxy<QueryStub, object>(
                    queryService: queryServiceStub,
                    cacheManager: cacheManagerStub.Object,
                    options: optionsStub,
                    userContext: userContextStub.Object);

            // Assert
            var actualCacheOptions = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(CacheOptions))
                .GetValue(sut);

            Assert.AreSame(optionsStub, actualCacheOptions);
        }
    }
}
