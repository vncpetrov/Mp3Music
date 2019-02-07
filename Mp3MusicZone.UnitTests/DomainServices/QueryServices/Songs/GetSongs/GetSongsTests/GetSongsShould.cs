namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.Songs.GetSongs.GetSongsTests
{
    using Mp3MusicZone.DomainServices.Contracts; 
    using NUnit.Framework;
    using System;
    using System.Linq;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongs;

    [TestFixture]
    public class GetSongsShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetSongs)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }
    }
}
