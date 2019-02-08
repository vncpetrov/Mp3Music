namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetForDeleteById.GetSongForDeleteByIdTests
{
    using Mp3MusicZone.Common.Constants;
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetForDeleteById;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class GetSongForDeleteByIdShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetSongForDeleteById)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }

        [Test]
        public void RequireCorrectPermission()
        {
            string expectedPermission = Permissions.DeleteSong;

            // Arrange && Act
            PermissionAttribute attr = typeof(GetSongForDeleteById)
                .GetCustomAttribute<PermissionAttribute>(false);

            string actualPermission = attr.PermissionId;

            // Assert
            Assert.AreEqual(expectedPermission, actualPermission);
        }
    }
}
