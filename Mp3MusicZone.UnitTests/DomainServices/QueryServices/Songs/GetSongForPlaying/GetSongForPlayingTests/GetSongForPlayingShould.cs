namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetSongForPlaying.GetSongForPlayingTests
{
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongForPlaying;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class GetSongForPlayingShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetSongForPlaying)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }
    }
}
