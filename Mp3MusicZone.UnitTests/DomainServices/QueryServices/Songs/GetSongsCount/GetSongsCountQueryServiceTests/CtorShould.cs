namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetSongsCount.GetSongsCountQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongsCount;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedSongRepositoryIsNull()
        {
            //var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetSongsCountQueryService(
                    songRepository: null));
        }

        [Test]
        public void SavePassedSongRepositoryWhenIsNotNull()
        {
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act
            GetSongsCountQueryService sut =
                new GetSongsCountQueryService(
                    songRepository: songRepositoryStub.Object);

            // Assert
            var actualSongProvider = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<Song>))
                .GetValue(sut);

            Assert.AreSame(songRepositoryStub.Object, actualSongProvider);
        }
    }
}
