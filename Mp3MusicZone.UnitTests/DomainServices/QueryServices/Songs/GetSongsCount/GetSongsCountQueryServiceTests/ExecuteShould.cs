namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetSongsCount.GetSongsCountQueryServiceTests
{
    using MockQueryable.Moq;
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongsCount;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public async Task ReturnTheCorrectCountOfApprovedSongsOnlyWhenInvoked()
        { 
            int expectedSongsCount = 1;

            var songs = new[]
            {
                new Song(){ IsApproved = false, Title = string.Empty },
                new Song(){ IsApproved = false, Title = string.Empty },
                new Song(){ IsApproved = false, Title = string.Empty },
                new Song(){ IsApproved = true, Title = string.Empty }
            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetSongsCount query = new GetSongsCount()
            {
                Approved = true,
                SearchInfo = new SearchInfo(null)
            };

            // Arrange
            GetSongsCountQueryService sut =
                new GetSongsCountQueryService(songRepositoryStub.Object);

            // Act
            int actualSongsCount = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(expectedSongsCount, actualSongsCount);
        }

        [Test]
        public async Task ReturnTheCorrectCountOfUnapprovedSongsOnlyWhenInvoked()
        { 
            int expectedSongsCount = 3;

            var songs = new[]
            {
                new Song(){ IsApproved = false, Title = string.Empty },
                new Song(){ IsApproved = false, Title = string.Empty },
                new Song(){ IsApproved = false, Title = string.Empty },
                new Song(){ IsApproved = true, Title = string.Empty }
            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetSongsCount query = new GetSongsCount()
            {
                Approved = false,
                SearchInfo = new SearchInfo(null)
            };

            // Arrange
            GetSongsCountQueryService sut =
                new GetSongsCountQueryService(songRepositoryStub.Object);

            // Act
            int actualSongsCount = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(expectedSongsCount, actualSongsCount);
        }

        [Test]
        public async Task ReturnCorrectCountOfSongsWhenThePassedQueryContainsNonEmptySearchTerm()
        {
            int expectedSongsCount = 1;

            var songs = new[]
            {
                new Song() { IsApproved = false, Title = "Aenean tempus" },
                new Song() { IsApproved = false, Title = "Odio lacus" },
                new Song() { IsApproved = false, Title = "Fusce eu libero" },
                new Song() { IsApproved = true, Title = "Aenean elementum" }
            }
            .AsQueryable()
            .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            GetSongsCount query = new GetSongsCount()
            {
                Approved = false,
                SearchInfo = new SearchInfo("aenean")
            };

            // Arrange
            GetSongsCountQueryService sut =
                new GetSongsCountQueryService(songRepositoryStub.Object);

            // Act
            int actualSongsCount = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(expectedSongsCount, actualSongsCount);
        }
    }
}
