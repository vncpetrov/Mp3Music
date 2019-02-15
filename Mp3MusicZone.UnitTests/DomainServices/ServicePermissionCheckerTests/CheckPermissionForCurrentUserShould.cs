namespace Mp3MusicZone.UnitTests.DomainServices.ServicePermissionCheckerTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.DomainServices;
    using Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CheckPermissionForCurrentUserShould
    {
        [Test]
        // UsePassedUserPermissionCheckerToCheckTheUserPermissions
        public void UsePassedUserPermissionCheckerToCheckPermissionsOfTheUser()
        {
            var permissionCheckerMock = new Mock<IUserPermissionChecker>();

            // Arrange
            ServicePermissionChecker<QueryStub> sut =
                new ServicePermissionChecker<QueryStub>(
                    permissionChecker: permissionCheckerMock.Object);

            // Act
            sut.CheckPermissionForCurrentUser();

            // Assert
            permissionCheckerMock.Verify(
                checker => checker.CheckPermission(
                    It.IsAny<string>()), Times.Once);
        }
    }
}
