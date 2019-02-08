namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetLastApproved.GetLastApprovedSongTests
{
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetLastApproved;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class GetLastApprovedSongsShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetLastApprovedSongs)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }
    }
}
