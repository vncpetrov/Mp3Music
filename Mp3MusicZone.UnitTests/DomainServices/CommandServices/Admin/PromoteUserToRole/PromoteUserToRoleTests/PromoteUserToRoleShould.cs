namespace Mp3MusicZone.UnitTests.DomainServices.CommandServices.
    Admin.PromoteUserToRole.PromoteUserToRoleTests
{
    using Mp3MusicZone.Common.Constants;
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.DomainServices.CommandServices.Admin.PromoteUserToRole;
    using NUnit.Framework;
    using System;
    using System.Reflection;

    [TestFixture]
    public class PromoteUserToRoleShould
    {
        [Test]
        public void RequireCorrectPermission()
        {
            string expectedPermission = Permissions.PromoteUserToRole;

            // Arrange && Act
            PermissionAttribute attr = typeof(PromoteUserToRole)
                .GetCustomAttribute<PermissionAttribute>(false);

            string actualPermission = attr.PermissionId;

            // Assert
            Assert.AreEqual(expectedPermission, actualPermission);
        }
    }
}
