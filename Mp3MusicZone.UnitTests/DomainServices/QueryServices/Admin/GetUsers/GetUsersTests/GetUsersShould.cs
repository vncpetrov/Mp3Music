namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.Admin.GetUsers.GetUsersTests
{
    using Mp3MusicZone.Common.Constants;
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Admin.GetUsers;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class GetUsersShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetUsers)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }

        [Test]
        public void RequireCorrectPermission()
        {
            string expectedPermission = Permissions.GetUsers;

            // Arrange && Act
            PermissionAttribute attr = typeof(GetUsers)
                .GetCustomAttribute<PermissionAttribute>(false);

            string actualPermission = attr.PermissionId;

            // Assert
            Assert.AreEqual(expectedPermission, actualPermission);
        }
    }
}
