namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetSongs.GetSongsQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongs;
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
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetSongsQueryService(
                    songRepository: null));
        }

        [Test]
        public void SavePassedSongRepositoryWhenIsNotNull()
        {
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act
            GetSongsQueryService sut =
                new GetSongsQueryService(
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
