namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.Uploader
    .GetUnapprovedSongForPlaying.GetUnapprovedSongForPlayingQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Exceptions;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongForPlaying;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public void ThrowsNotFoundExceptionWhenTheSongDoesNotExists()
        {
            var songProviderStub = new Mock<ISongProvider>();
            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Song)null);

            GetUnapprovedSongForPlaying query = new GetUnapprovedSongForPlaying();

            // Arrange
            GetUnapprovedSongForPlayingQueryService sut = new GetUnapprovedSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(query));
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
                .ReturnsAsync(new Song());

            GetUnapprovedSongForPlaying query = new GetUnapprovedSongForPlaying();

            // Arrange
            GetUnapprovedSongForPlayingQueryService sut = new GetUnapprovedSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Act
            byte[] actualSongFile = (await sut.ExecuteAsync(query)).File;

            // Assert
            Assert.AreSame(expectedSongFile, actualSongFile);
        }

        [Test]
        public async Task ReturnDtoWithCorrectSongExtensionWhenTheSongExists()
        {
            string expectedSongExtension = ".mp3";

            var songProviderStub = new Mock<ISongProvider>();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Song() { FileExtension = expectedSongExtension });

            GetUnapprovedSongForPlaying query = new GetUnapprovedSongForPlaying();

            // Arrange
            GetUnapprovedSongForPlayingQueryService sut = new GetUnapprovedSongForPlayingQueryService(
                songProvider: songProviderStub.Object,
                songRepository: songRepositoryStub.Object);

            // Act
            string actualSongExtension = (await sut.ExecuteAsync(query)).FileExtension;

            // Assert
            Assert.AreSame(expectedSongExtension, actualSongExtension);
        }
    }
}
