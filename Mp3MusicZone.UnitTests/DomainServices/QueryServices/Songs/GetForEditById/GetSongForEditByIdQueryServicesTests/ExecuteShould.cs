namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetForEditById.GetSongForEditByIdQueryServicesTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Exceptions;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetForEditById;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public void ThrowsNotFoundExceptionWhenSongDoesNotExists()
        {
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Song)null);

            GetSongForEditById query = new GetSongForEditById();

            // Arrange
            GetSongForEditByIdQueryService sut = new GetSongForEditByIdQueryService(
                songRepository: songRepositoryStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(query));
        }

        [Test]
        public async Task ReturnsCorrectSongWhenTheSongExists()
        {
            Song expectedSong = new Song()
            {
                Id = "SongId"
            };

            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            songRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedSong);

            GetSongForEditById query = new GetSongForEditById();

            // Arrange
            GetSongForEditByIdQueryService sut = new GetSongForEditByIdQueryService(
                songRepository: songRepositoryStub.Object);

            // Act
            Song actualSong = await sut.ExecuteAsync(query);

            // Act && Assert
            Assert.AreEqual(expectedSong.Id, actualSong.Id);
        }
    }
}
