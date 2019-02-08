namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetSongForPlaying.GetSongForPlayingQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Exceptions;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongForPlaying;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public void ThrowsNotFoundExceptionWhenSongDoesNotExists()
        {
            var songProviderStub = new Mock<ISongProvider>();
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Song)null);

            GetSongForPlaying query = new GetSongForPlaying()
            {
                SongId = "Invalid Id"
            };

            // Arrange
            GetSongForPlayingQueryService sut = new GetSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(query));
        }

        [Test]
        public void ThrowsNotFoundExceptionWhenSongIsNotApprovedExists()
        {
            var songProviderStub = new Mock<ISongProvider>();
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Song() { Id = "1", IsApproved = false });

            GetSongForPlaying query = new GetSongForPlaying()
            {
                SongId = "1"
            };

            // Arrange
            GetSongForPlayingQueryService sut = new GetSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(query));
        }

        [Test]
        public async Task ReturnDtoWithCorrectSongExtensionWhenTheSongExists()
        {
            string expectedSongExtension = ".mp3";

            var songProviderStub = new Mock<ISongProvider>();
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    new Song()
                    {
                        IsApproved = true,
                        FileExtension = expectedSongExtension
                    });

            GetSongForPlaying query = new GetSongForPlaying();

            // Arrange
            GetSongForPlayingQueryService sut = new GetSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Act 
            string actualSongExtension = (await sut.ExecuteAsync(query)).FileExtension;

            // Assert
            Assert.AreEqual(expectedSongExtension, actualSongExtension);
        }

        [Test]
        public async Task ReturnDtoWithCorrectSongFileWhenTheSongExists()
        {
            byte[] expectedSongFile = new byte[128];

            var songProviderStub = new Mock<ISongProvider>();
            songProviderStub
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => expectedSongFile);

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Song() { IsApproved = true });

            GetSongForPlaying query = new GetSongForPlaying();

            // Arrange
            GetSongForPlayingQueryService sut = new GetSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Act 
            byte[] actualSongFile = (await sut.ExecuteAsync(query)).File;

            // Assert
            Assert.AreEqual(expectedSongFile, actualSongFile);
        }

        [Test]
        public async Task ReturnDtoWithCorrectSongWhenTheSongExists()
        {
            Song expectedSong = new Song()
            {
                Title = "Unit Test",
                IsApproved = true
            };

            var songProviderStub = new Mock<ISongProvider>();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync(expectedSong);

            GetSongForPlaying query = new GetSongForPlaying();

            // Arrange
            GetSongForPlayingQueryService sut = new GetSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Act 
            SongForPlayingDTO actualSongDTO = await sut.ExecuteAsync(query);

            // Assert
            StringAssert.Contains(expectedSong.Title, actualSongDTO.HeadingText);
        }
    }
}
