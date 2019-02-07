namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices
    .Songs.GetSongsCount.GetSongsCountTests
{
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongsCount;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class GetSongsCountShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetSongsCount)
                .GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }
    }
}
