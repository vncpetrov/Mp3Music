namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetApprovedSongsByUser.GetApprovedSongsByUserQueryServiceTests
{
    using MockQueryable.Moq;
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Exceptions;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetApprovedSongsByUser;
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public void ThrowsNotFoundExceptionWhenUserDoesNotExists()
        {
            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            var userRepositoryStub = new Mock<IEfRepository<User>>();

            userRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            GetApprovedSongsByUser query = new GetApprovedSongsByUser();

            // Arrange  
            GetApprovedSongsByUserQueryService sut =
                new GetApprovedSongsByUserQueryService(
                    songRepository: songRepositoryStub.Object,
                    userRepository: userRepositoryStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(query));
        }

        [Test]
        public async Task ReturnsApprovedSongsOnlyWhenTheUserExists()
        {
            const string SomeId = "SomeId";

            var songs = new[]
            {
                new Song(){ IsApproved = false, UploaderId = SomeId },
                new Song(){ IsApproved = true, UploaderId = SomeId },
                new Song(){ IsApproved = true, UploaderId = SomeId },
                new Song(){ IsApproved = true, UploaderId = SomeId },
            }
           .AsQueryable()
           .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub.Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            var userRepositoryStub = new Mock<IEfRepository<User>>();

            userRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { Id = SomeId });

            GetApprovedSongsByUser query = new GetApprovedSongsByUser()
            {
                UserId = SomeId
            };

            // Arrange  
            GetApprovedSongsByUserQueryService sut =
                new GetApprovedSongsByUserQueryService(
                    songRepository: songRepositoryStub.Object,
                    userRepository: userRepositoryStub.Object);

            // Act 
            IEnumerable<Song> actualSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.That(actualSongs.All(s => s.IsApproved = true));
        }

        [Test]
        public async Task ReturnsSongsOnlyFromUserWithPassedUserId()
        {
            const string SomeId = "SomeId"; 

            var songs = new[]
            {
                new Song(){ IsApproved = false, UploaderId = SomeId },
                new Song(){ IsApproved = true, UploaderId = SomeId },
                new Song(){ IsApproved = true, UploaderId = SomeId },
                new Song(){ IsApproved = true, UploaderId = SomeId },
            }
           .AsQueryable()
           .BuildMock();

            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            songRepositoryStub.Setup(x => x.All(It.IsAny<bool>()))
                .Returns(songs.Object);

            var userRepositoryStub = new Mock<IEfRepository<User>>();

            userRepositoryStub
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { Id = SomeId });

            GetApprovedSongsByUser query = new GetApprovedSongsByUser()
            {
                UserId = SomeId
            };

            // Arrange  
            GetApprovedSongsByUserQueryService sut =
                new GetApprovedSongsByUserQueryService(
                    songRepository: songRepositoryStub.Object,
                    userRepository: userRepositoryStub.Object);

            // Act 
            IEnumerable<Song> actualSongs = await sut.ExecuteAsync(query);

            // Assert
            Assert.That(actualSongs.All(s => s.UploaderId == SomeId));
        }
    }
}
