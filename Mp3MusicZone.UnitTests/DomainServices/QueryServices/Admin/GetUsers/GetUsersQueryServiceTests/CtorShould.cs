namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Admin.GetUsers.GetUsersQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Admin.GetUsers;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedUserRepositoryIsNull()
        {  
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetUsersQueryService(
                    userRepository:  null));
        }

        [Test]
        public void SavePassedUserRepositoryWhenIsNotNull()
        { 
            var userRepositoryStub = new Mock<IEfRepository<User>>();

            // Arrange && Act
            GetUsersQueryService sut =
                new GetUsersQueryService(
                    userRepository: userRepositoryStub.Object);

            // Assert
            var actualUserRepository = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<User>))
                .GetValue(sut);

            Assert.AreSame(userRepositoryStub.Object, actualUserRepository);
        }
    }
}
