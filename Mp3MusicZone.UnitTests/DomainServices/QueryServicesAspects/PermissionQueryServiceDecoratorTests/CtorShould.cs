namespace Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.
    PermissionQueryServiceDecoratorTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.DomainServices;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServicesAspects;
    using Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedPermissionCheckerIsNull()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new PermissionQueryServiceDecorator<QueryStub, object>(
                    permissionChecker: null,
                    decoratee: new QueryServiceStub()));
        }

        [Test]
        public void SavePassedPermissionCheckerWhenIsNotNull()
        {
            var userPermissionCheckerStub = new Mock<IUserPermissionChecker>();
            var permissionCheckerStub = new ServicePermissionCheckerStub(
                permissionChecker: userPermissionCheckerStub.Object);

            QueryServiceStub decorateeStub = new QueryServiceStub();

            // Arrange && Act
            PermissionQueryServiceDecorator<QueryStub, object> sut = new PermissionQueryServiceDecorator<QueryStub, object>(
                permissionChecker: permissionCheckerStub,
                decoratee: decorateeStub);

            // Assert
            var actualPermissionChecker = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(ServicePermissionChecker<QueryStub>))
                .GetValue(sut);

            Assert.AreSame(permissionCheckerStub, actualPermissionChecker);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedDecorateeIsNull()
        {
            var userPermissionCheckerStub = new Mock<IUserPermissionChecker>();
            var permissionCheckerStub = new ServicePermissionCheckerStub(
                permissionChecker: userPermissionCheckerStub.Object);

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new PermissionQueryServiceDecorator<QueryStub, object>(
                    permissionChecker: permissionCheckerStub,
                    decoratee: null));
        }

        [Test]
        public void SavePassedDecorateeWhenIsNotNull()
        {
            var userPermissionCheckerStub = new Mock<IUserPermissionChecker>();
            var permissionCheckerStub = new ServicePermissionCheckerStub(
                permissionChecker: userPermissionCheckerStub.Object);

            QueryServiceStub decoratee = new QueryServiceStub();

            // Arrange && Act
            PermissionQueryServiceDecorator<QueryStub, object> sut = new PermissionQueryServiceDecorator<QueryStub, object>(
                permissionChecker: permissionCheckerStub,
                decoratee: decoratee);

            // Assert
            var actualDecoratee = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IQueryService<QueryStub, object>))
                .GetValue(sut);

            Assert.AreSame(decoratee, actualDecoratee);
        }
    }
}
