namespace Mp3MusicZone.UnitTests.DomainServices.CommandServices.
    Admin.DemoteUserFromRole.DemoteUserFromRoleTests
{
    using Mp3MusicZone.Common.Constants;
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.DomainServices.CommandServices.Admin.DemoteUserFromRole;
    using NUnit.Framework;
    using System;
    using System.Reflection;

    [TestFixture]
    public class DemoteUserFromRoleShould
    {
        [Test]
        public void RequireCorrectPermission()
        {
            string expectedPermission = Permissions.DemoteUserFromRole;

            // Arrange && Act
            PermissionAttribute attr = typeof(DemoteUserFromRole)
                .GetCustomAttribute<PermissionAttribute>(false);

            string actualPermission = attr.PermissionId;

            // Assert
            Assert.AreEqual(expectedPermission, actualPermission);
        }
    }
}
