namespace Mp3MusicZone.UnitTests.DomainServices.CommandServices.
    Admin.PromoteUserToRole.PromoteUserToRoleCommandServiceTests
{
    using NUnit.Framework;
    using Mp3MusicZone.DomainServices.CommandServices.Admin.PromoteUserToRole;
    using System;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.Domain.Contracts;
    using Moq;
    using Mp3MusicZone.EfDataAccess;
    using System.Linq;
    using MockQueryable.Moq;
    using Mp3MusicZone.Domain.Exceptions;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public void ThrowsNotFoundExceptionWhenUserDoesNotExists()
        {
            var usersStub = new User[0]
                .AsQueryable()
                .BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub.Setup(r => r.All(It.IsAny<bool>()))
                .Returns(usersStub.Object);

            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            PromoteUserToRole command = new PromoteUserToRole()
            {
                UserId = "InvalidUserId"
            };

            // Arrange 
            PromoteUserToRoleCommandService sut =
                new PromoteUserToRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await sut.ExecuteAsync(command));
        }

        [Test]
        public void ThrowsNotFoundExceptionWhenTheRoleDoesNotExists()
        {
            const string roleName = "RoleName";
            const string userId = "UserId";

            var usersStub = new[]
            {
                 new User()
                 {
                     Id = userId
                 }
             }
            .AsQueryable()
            .BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub.Setup(r => r.All(It.IsAny<bool>()))
                .Returns(usersStub.Object);

            var rolesStub = new[]
            {
                new Role() { Name = roleName }
            }
            .AsQueryable()
            .BuildMock();

            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            roleRepositoryStub.Setup(r => r.All(It.IsAny<bool>()))
                .Returns(rolesStub.Object);

            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            PromoteUserToRole command = new PromoteUserToRole()
            {
                UserId = userId,
                RoleName = "AnotherRoleName"
            };

            // Arrange 
            PromoteUserToRoleCommandService sut =
                new PromoteUserToRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Act && Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(
                async () => await sut.ExecuteAsync(command));

            StringAssert.Contains("does not exists", ex.Message);
        }

        [Test]
        public async Task AddRoleToUserRolesWhenCommandContainsValidData()
        {
            const string roleName = "RoleName";
            const string userId = "UserId";

            List<Role> userRoles = new List<Role>();

            var usersStub = new[]
            {
                 new User()
                 {
                     Id = userId,
                     Roles = userRoles
                 }
             }
            .AsQueryable()
            .BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub.Setup(r => r.All(It.IsAny<bool>()))
                .Returns(usersStub.Object);

            var rolesStub = new[]
            {
                new Role() { Name = roleName }
            }
            .AsQueryable()
            .BuildMock();

            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            roleRepositoryStub.Setup(r => r.All(It.IsAny<bool>()))
                .Returns(rolesStub.Object);

            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            PromoteUserToRole command = new PromoteUserToRole()
            {
                UserId = userId, 
                RoleName = roleName
            };

            // Arrange 
            PromoteUserToRoleCommandService sut =
                new PromoteUserToRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);
              
            // Act
            await sut.ExecuteAsync(command);

            // Assert
            Assert.AreEqual(1, userRoles.Count);
        }
    }
}
