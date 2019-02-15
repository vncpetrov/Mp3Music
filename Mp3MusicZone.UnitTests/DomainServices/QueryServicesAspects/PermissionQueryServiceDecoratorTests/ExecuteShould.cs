namespace Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.
    PermissionQueryServiceDecoratorTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServicesAspects;
    using Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public async Task CheckPermissionsOfTheUserWhenInvoked()
        {
            var userPermissionCheckerMock = new Mock<IUserPermissionChecker>();
            var permissionCheckerStub = new ServicePermissionCheckerStub(
                permissionChecker: userPermissionCheckerMock.Object);
            
            QueryServiceStub decorateeStub = new QueryServiceStub();

            // Arrange 
            PermissionQueryServiceDecorator<QueryStub, object> sut = 
                new PermissionQueryServiceDecorator<QueryStub, object>(
                    permissionChecker: permissionCheckerStub,
                    decoratee: decorateeStub);

            // Act
            await sut.ExecuteAsync(new QueryStub());

            // Assert
            userPermissionCheckerMock.Verify(
                x => x.CheckPermission(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CallThePassedDecoratee()
        {
            var userPermissionCheckerMock = new Mock<IUserPermissionChecker>();
            var permissionCheckerStub = new ServicePermissionCheckerStub(
                permissionChecker: userPermissionCheckerMock.Object);

            var deorateeMock = new Mock<IQueryService<QueryStub, object>>();

            // Arrange 
            PermissionQueryServiceDecorator<QueryStub, object> sut =
                new PermissionQueryServiceDecorator<QueryStub, object>(
                    permissionChecker: permissionCheckerStub,
                    decoratee: deorateeMock.Object);

            // Act
            await sut.ExecuteAsync(new QueryStub());

            // Assert
            deorateeMock.Verify(
                x => x.ExecuteAsync(It.IsAny<QueryStub>()), Times.Once);
        }
    }
}
