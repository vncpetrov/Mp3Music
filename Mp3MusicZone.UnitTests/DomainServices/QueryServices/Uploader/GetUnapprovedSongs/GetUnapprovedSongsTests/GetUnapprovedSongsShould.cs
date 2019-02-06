namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices
    .Uploader.GetUnapprovedSongs.GetUnapprovedSongsTests
{
    using Mp3MusicZone.Common.Constants;
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongs;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class GetUnapprovedSongsShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetUnapprovedSongs)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }

        [Test]
        public void RequireCorrectPermission()
        {
            string expectedPermission = Permissions.GetUnapprovedSongs;

            // Arrange && Act
            PermissionAttribute attr = typeof(GetUnapprovedSongs)
                .GetCustomAttribute<PermissionAttribute>(false);

            string actualPermission = attr.PermissionId;

            // Assert
            Assert.AreEqual(expectedPermission, actualPermission);
        }
    }
}
