namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetSongs.GetSongsQueryServiceTests
{
    using MockQueryable.Moq;
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongs;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public async Task ReturnApprovedSongsOnlyWhenInvoked()
        {
            var songs = new[]
            {
                new Song(){ IsApproved = false, Title = string.Empty },
                new Song(){ IsApproved = true, Title = string.Empty },
                new Song(){ IsApproved = true, Title = string.Empty },
                new Song(){ IsApproved = true, Title = string.Empty },
            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetSongs query = new GetSongs()
            {
                PageInfo = new PageInfo(1, 5),
                SearchInfo = new SearchInfo(null)
            };

            // Arrange
            GetSongsQueryService sut =
                new GetSongsQueryService(songRepositoryStub.Object);

            // Act
            IEnumerable<Song> actualSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.That(actualSongs.All(s => s.IsApproved = true));
        }

        [Test]
        public async Task ReturnTheCorrectSongsWhenThePassedQueryContainsNonEmptySearchTerm()
        {
            var expectedSongs = new[]
            {
                new Song() { IsApproved = true, Title = "Aenean tempus" },
                new Song() { IsApproved = true, Title = "Aenean elementum" }
            };

            var songs = new List<Song>()
            {
                new Song() { IsApproved = true, Title = "Odio lacus" },
                new Song() { IsApproved = false, Title = "Fusce eu libero" }
            };

            songs.AddRange(expectedSongs);

            var songsStub = songs.AsQueryable()
                .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songsStub.Object);

            GetSongs query = new GetSongs()
            {
                PageInfo = new PageInfo(1, 5),
                SearchInfo = new SearchInfo("aenean")
            };

            // Arrange
            GetSongsQueryService sut =
                new GetSongsQueryService(songRepositoryStub.Object);

            // Act
            IEnumerable<Song> actualSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(2, actualSongs.Count());
            CollectionAssert.AreEqual(expectedSongs, actualSongs);
        }

        [Test]
        public async Task ReturnTheSongsOrderedByPublishDateDescendingWhenInvoked()
        {
            var expectedSongs = new[]
            {
                new Song()
                {
                    IsApproved = true,
                    Title = string.Empty,
                    PublishedOn = new DateTime(2000, 1, 3)
                },

                new Song()
                {
                    IsApproved = true,
                    Title = string.Empty,
                    PublishedOn = new DateTime(2000, 1, 2)
                },

                new Song()
                {
                    IsApproved = true,
                    Title = string.Empty,
                    PublishedOn = new DateTime(2000, 1, 1)
                }
            };

            var songs = expectedSongs.OrderBy(s => s.PublishedOn)
                .AsQueryable()
                .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetSongs query = new GetSongs()
            {
                PageInfo = new PageInfo(1, 5),
                SearchInfo = new SearchInfo(null)
            };

            // Arrange
            GetSongsQueryService sut =
                new GetSongsQueryService(songRepositoryStub.Object);

            // Act
            IEnumerable<Song> actualSongs = await sut.ExecuteAsync(query);

            // Assert
            CollectionAssert.AreEqual(expectedSongs, actualSongs);
        }
    }
}
