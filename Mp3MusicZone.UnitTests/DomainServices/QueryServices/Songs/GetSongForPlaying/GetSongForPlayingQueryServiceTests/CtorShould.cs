namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetSongForPlaying.GetSongForPlayingQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongForPlaying;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedSongProviderIsNull()
        {
            // Arrange
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetSongForPlayingQueryService(
                    songProvider: null,
                    songRepository: songRepositoryStub.Object));
        }

        [Test]
        public void SavePassedSongProviderWhenIsNotNull()
        {
            var songProviderStub = new Mock<ISongProvider>();
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act
            GetSongForPlayingQueryService sut = new GetSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Assert
            var actualSongProvider = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(ISongProvider))
                .GetValue(sut);

            Assert.AreSame(songProviderStub.Object, actualSongProvider); 
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedSongRepositoryIsNull()
        {
            // Arrange
            var songProviderStub = new Mock<ISongProvider>();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetSongForPlayingQueryService(
                    songProvider: songProviderStub.Object,
                    songRepository: null));
        }

        [Test]
        public void SavePassedSongRepositoryWhenIsNotNull()
        {
            var songProviderStub = new Mock<ISongProvider>();
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act
            GetSongForPlayingQueryService sut = new GetSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Assert
            var actualSongRepository = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<Song>))
                .GetValue(sut);

            Assert.AreSame(songRepositoryStub.Object, actualSongRepository);
        }
    }
}
