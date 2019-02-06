namespace Mp3MusicZone.
    UnitTests.
    DomainServices.
    QueryServices.
    Users.
    GetUserCount.
    GetUserCountQueryServiceTests
{
    using MockQueryable.Moq;
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices;
    using Mp3MusicZone.DomainServices.QueryServices.Users.GetUsersCount;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public async Task UsePassedUserRepositoryWhenInvoked()
        {
            // Arrange
            var users = new[]
            {
                new User{ UserName = "Alex" },
                new User{ UserName = "Peter" },
                new User{ UserName = "Johnny" }
            }
            .AsQueryable()
            .BuildMock();

            var userRepositoryMock = new Mock<IEfRepository<User>>();
            userRepositoryMock.Setup(x => x.All(It.IsAny<bool>())).Returns(users.Object);

            GetUsersCountQueryService sut =
                new GetUsersCountQueryService(userRepositoryMock.Object);

            GetUsersCount query = new GetUsersCount()
            {
                SearchInfo = new SearchInfo(null)
            };

            // Act
            await sut.ExecuteAsync(query);

            // Assert
            userRepositoryMock.Verify(ur => ur.All(It.IsAny<bool>()), Times.Once());
        }

        [Test]
        public async Task ReturnCountOfAllUsersInRepositoryWhenThePassedQueryContainsEmptySearchTerm()
        {
            var users = new[]
            {
                new User{ UserName = "Alex" },
                new User{ UserName = "Peter" },
                new User{ UserName = "Johnny" }
            }
            .AsQueryable()
            .BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(users.Object);

            int expectedUsersCount = 3;

            GetUsersCount query = new GetUsersCount()
            {
                SearchInfo = new SearchInfo(null)
            };

            // Arrange
            GetUsersCountQueryService sut =
                new GetUsersCountQueryService(userRepositoryStub.Object);

            // Act
            int actualUsersCount = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(expectedUsersCount, actualUsersCount);
        }

        [Test]
        public async Task ReturnCorrectCountOfUsersWhenThePassedQueryContainsNonEmptySearchTerm()
        {
            var users = new[]
            {
                new User { UserName = "Alex" },
                new User { UserName = "Peter" },
                new User { UserName = "Johnny" },
            }
            .AsQueryable()
            .BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(users.Object);

            int expectedUsersCount = 2;

            GetUsersCount query = new GetUsersCount()
            {
                SearchInfo = new SearchInfo("e")
            };

            // Arrange
            GetUsersCountQueryService sut =
                new GetUsersCountQueryService(userRepositoryStub.Object);

            // Act
            int actualUsersCount = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(expectedUsersCount, actualUsersCount);
        }
    }
}
