namespace Mp3MusicZone
    .UnitTests
    .DomainServices
    .QueryServices
    .Users
    .GetUserCount
    .GetUserCountQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Users.GetUsersCount;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenNullUserRepositoryIsPassed()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(() => new GetUsersCountQueryService(null));
        }

        [Test]
        public void SavePassedUserRepositoryWhenIsNotNull()
        {
            Mock<IEfRepository<User>> userRepositoryStub = new Mock<IEfRepository<User>>();

            // Arrange && Act
            GetUsersCountQueryService sut =
                new GetUsersCountQueryService(userRepositoryStub.Object);

            // Assert
            var actualUserRepository = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<User>))
                .GetValue(sut);
            
            Assert.AreSame(userRepositoryStub.Object, actualUserRepository);
        }
    }
}
