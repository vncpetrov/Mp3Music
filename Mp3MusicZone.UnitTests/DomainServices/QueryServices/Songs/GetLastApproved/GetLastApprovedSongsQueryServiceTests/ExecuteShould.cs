namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetLastApproved.GetLastApprovedSongsQueryServiceTests
{
    using MockQueryable.Moq;
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetLastApproved;
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
                new Song(){ IsApproved = false, Id = string.Empty },
                new Song(){ IsApproved = true, Id = string.Empty },
                new Song(){ IsApproved = true, Id = string.Empty },
                new Song(){ IsApproved = true, Id = string.Empty },
            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetLastApprovedSongs query = new GetLastApprovedSongs()
            {
                Count = 5
            };

            // Arrange
            GetLastApprovedSongsQueryService sut =
                new GetLastApprovedSongsQueryService(songRepositoryStub.Object);

            // Act
            IEnumerable<Song> actualSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.That(actualSongs.All(s => s.IsApproved = true));
        }

        [Test]
        public async Task ReturnTheSameCountOfSongsAsSpecifiedInThePassedQueryWhenInvoked()
        {
            int expectedSongsCount = 2;

            var songs = new[]
            {
                new Song(){ IsApproved = false, Id = string.Empty },
                new Song(){ IsApproved = true, Id = string.Empty },
                new Song(){ IsApproved = true, Id = string.Empty },
                new Song(){ IsApproved = true, Id = string.Empty },
            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetLastApprovedSongs query = new GetLastApprovedSongs()
            {
                Count = expectedSongsCount
            };

            // Arrange
            GetLastApprovedSongsQueryService sut =
                new GetLastApprovedSongsQueryService(songRepositoryStub.Object);

            // Act
            IEnumerable<Song> actualSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(expectedSongsCount, actualSongs.Count());
        }

        [Test]
        public async Task ReturnTheSongsOrderedByPublishDateDescendingWhenInvoked()
        {
            var songs = new[]
            {
                new Song(){ IsApproved = true, PublishedOn = new DateTime(2000, 1, 3) },
                new Song(){ IsApproved = true, PublishedOn = new DateTime(2000, 1, 2) },
                new Song(){ IsApproved = true, PublishedOn = new DateTime(2000, 1, 1) },
                new Song(){ IsApproved = true, PublishedOn = new DateTime(2000, 1, 21) }
            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetLastApprovedSongs query = new GetLastApprovedSongs()
            {
                Count = 5
            };

            // Arrange
            GetLastApprovedSongsQueryService sut =
                new GetLastApprovedSongsQueryService(songRepositoryStub.Object);

            // Act
            IEnumerable<Song> actualSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.That(
                actualSongs
                    .Zip(actualSongs.Skip(1), (a, b) => new { a, b })
                        .All(s => s.a.PublishedOn > s.b.PublishedOn));
        }
    }
}
