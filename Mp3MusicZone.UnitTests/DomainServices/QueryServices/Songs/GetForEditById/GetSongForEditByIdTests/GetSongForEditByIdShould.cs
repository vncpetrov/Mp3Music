namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetForEditById.GetSongForEditByIdTests
{
    using Mp3MusicZone.Common.Constants;
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetForEditById;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class GetSongForEditByIdShould
    { 
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetSongForEditById)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }

        [Test]
        public void RequireCorrectPermission()
        {
            string expectedPermission = Permissions.EditSong;

            // Arrange && Act
            PermissionAttribute attr = typeof(GetSongForEditById)
                .GetCustomAttribute<PermissionAttribute>(false);

            string actualPermission = attr.PermissionId;

            // Assert
            Assert.AreEqual(expectedPermission, actualPermission);
        }
    }
}
