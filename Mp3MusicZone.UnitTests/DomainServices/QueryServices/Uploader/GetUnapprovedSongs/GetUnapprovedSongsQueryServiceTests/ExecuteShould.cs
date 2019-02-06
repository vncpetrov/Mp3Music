namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.Uploader
    .GetUnapprovedSongs.GetUnapprovedSongsQueryServiceTests
{
    using MockQueryable.Moq;
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices;
    using Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongs;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public async Task ReturnsOnlyUnapprovedSongsWhenInvoked()
        {
            var songs = new[]
            {
                new Song { IsApproved = false },
                new Song { IsApproved = true },
                new Song { IsApproved = true },
                new Song { IsApproved = false },
                new Song { IsApproved = false },

            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetUnapprovedSongs query = new GetUnapprovedSongs()
            {
                PageInfo = new PageInfo(1, 5)
            };

            // Arrange
            GetUnapprovedSongsQueryService sut =
                new GetUnapprovedSongsQueryService(songRepositoryStub.Object);

            // Act
            IEnumerable<Song> actualUnapprovedSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.That(actualUnapprovedSongs.All(s => s.IsApproved == false));
        }

        [Test]
        public async Task ReturnsCorrectUnapprovedSongsWhenInvoked()
        {
            Song expectedSong = new Song { Id = "5" };

            var songs = new[]
            {
                new Song { Id = "1" },
                new Song { Id = "2" },
                new Song { Id = "3" },
                new Song { Id = "4" },
                expectedSong
            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetUnapprovedSongs query = new GetUnapprovedSongs()
            {
                PageInfo = new PageInfo(3, 2)
            };

            // Arrange
            GetUnapprovedSongsQueryService sut =
                new GetUnapprovedSongsQueryService(songRepositoryStub.Object);

            // Act
            IEnumerable<Song> actualUnapprovedSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(1, actualUnapprovedSongs.Count());
            CollectionAssert.Contains(actualUnapprovedSongs, expectedSong);
        }
    }
}
