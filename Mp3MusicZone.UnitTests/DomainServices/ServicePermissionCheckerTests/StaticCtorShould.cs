namespace Mp3MusicZone.UnitTests.DomainServices.ServicePermissionCheckerTests
{
    using Moq;
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.DomainServices;
    using Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class StaticCtorShould
    {
        [Test]
        public void ExtractPermissionIdOfGivenGenericTypeParameter()
        {
            string expectedPermissionId =
                typeof(QueryStub)
                .GetCustomAttribute<PermissionAttribute>()
                .PermissionId;

            var permissionCheckerStub = new Mock<IUserPermissionChecker>();

            // Arrange && Act
            ServicePermissionChecker<QueryStub> sut =
                new ServicePermissionChecker<QueryStub>(
                    permissionChecker: permissionCheckerStub.Object);

            // Assert
            var actualPermissionId = sut.GetType()
                .GetFields(BindingFlags.Static | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(string))
                .GetValue(sut);

            Assert.AreEqual(expectedPermissionId, actualPermissionId);
        }

        [Test]
        public void ThrowsInvalidOperationExceptionWhenGivenGenericTypeParameterIsNotMarkedWithThePermissionAttribute()
        {
            var permissionCheckerStub = new Mock<IUserPermissionChecker>();

            // Arrange && Act
            TypeInitializationException ex = Assert.Throws<TypeInitializationException>(() =>
                new ServicePermissionChecker<QueryStubWithoutPermissionAttributeStub>(
                    permissionChecker: permissionCheckerStub.Object));

            // Assert
            Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        }
    }
}
