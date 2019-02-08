namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Admin.GetUsers.GetUsersQueryServiceTests
{
    using MockQueryable.Moq;
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices;
    using Mp3MusicZone.DomainServices.QueryServices.Admin.GetUsers;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public async Task ReturnTheCorrectSongsWhenThePassedQueryContainsNonEmptySearchTerm()
        {
            var expectedUsers = new[]
            {
                new User() { UserName = "Odio eu lacus"  },
                new User() { UserName = "Fusce eu libero" }
            };

            var users = new List<User>()
            {
                new User() { UserName = "Sed consequat"  },
                new User() { UserName = "Tellus praesent" }
            };

            users.AddRange(expectedUsers);

            var usersStub = users.AsQueryable()
                .BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub
                .Setup(x => x.All(It.IsAny<bool>()))
                .Returns(usersStub.Object);

            GetUsers query = new GetUsers()
            {
                PageInfo = new PageInfo(1, 5),
                SearchInfo = new SearchInfo("eu")
            };

            // Arrange
            GetUsersQueryService sut =
                new GetUsersQueryService(userRepositoryStub.Object);

            // Act
            IEnumerable<User> actualUsers = await sut.ExecuteAsync(query);

            // Assert
            Assert.AreEqual(2, actualUsers.Count());
            CollectionAssert.AreEqual(expectedUsers, actualUsers);
        }
    }
}
