namespace Mp3MusicZone.UnitTests.DomainServices.ServicePermissionCheckerTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.DomainServices;
    using Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedUserPermissionCheckerIsNull()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new ServicePermissionChecker<QueryStub>(
                    permissionChecker: null));
        }

        [Test]
        public void SavePassedUserPermissionCheckerWhenIsNotNull()
        {
            var permissionCheckerStub = new Mock<IUserPermissionChecker>();

            // Arrange && Act
            ServicePermissionChecker<QueryStub> sut = 
                new ServicePermissionChecker<QueryStub>(
                    permissionChecker: permissionCheckerStub.Object);

            // Assert
            var actualPermissionChecker = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IUserPermissionChecker))
                .GetValue(sut);

            Assert.AreSame(permissionCheckerStub.Object, actualPermissionChecker);
        }
    }
}
