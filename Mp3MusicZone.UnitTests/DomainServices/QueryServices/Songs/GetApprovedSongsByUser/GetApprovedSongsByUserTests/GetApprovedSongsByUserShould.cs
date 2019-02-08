namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetApprovedSongsByUser.GetApprovedSongsByUserTests
{
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetApprovedSongsByUser;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class GetApprovedSongsByUserShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetApprovedSongsByUser)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }
    }
}
