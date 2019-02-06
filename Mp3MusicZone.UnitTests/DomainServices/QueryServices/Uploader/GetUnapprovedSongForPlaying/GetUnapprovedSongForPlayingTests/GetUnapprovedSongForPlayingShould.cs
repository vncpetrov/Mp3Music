namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.Uploader
    .GetUnapprovedSongForPlaying.GetUnapprovedSongForPlayingTests
{
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongForPlaying;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;
    using Mp3MusicZone.Common.Constants;

    [TestFixture]
    public class GetUnapprovedSongForPlayingShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetUnapprovedSongForPlaying)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }

        [Test]
        public void RequireCorrectPermission()
        {
            string expectedPermission = Permissions.GetUnapprovedSongForPlaying;

            // Arrange && Act
            PermissionAttribute attr = typeof(GetUnapprovedSongForPlaying)
                .GetCustomAttribute<PermissionAttribute>(false);

            string actualPermission = attr.PermissionId;

            // Assert
            Assert.AreEqual(expectedPermission, actualPermission);
        }
    }
}
