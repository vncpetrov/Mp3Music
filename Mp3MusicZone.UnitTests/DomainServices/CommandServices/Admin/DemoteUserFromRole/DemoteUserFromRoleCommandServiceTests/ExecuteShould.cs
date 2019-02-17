namespace Mp3MusicZone.UnitTests.DomainServices.CommandServices.
    Admin.DemoteUserFromRole.DemoteUserFromRoleCommandServiceTests
{
    using MockQueryable.Moq;
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Exceptions;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.Domain.Models.Enums;
    using Mp3MusicZone.DomainServices.CommandServices.Admin.DemoteUserFromRole;
    using Mp3MusicZone.EfDataAccess;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExecuteShould
    {
        [Test]
        public void ThrowsNotFoundExceptionWhenUserDoesNotExists()
        {
            var usersStub = new User[0].AsQueryable().BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub.Setup(r => r.All(It.IsAny<bool>()))
                .Returns(usersStub.Object);

            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            DemoteUserFromRole command = new DemoteUserFromRole()
            {
                UserId = "InvalidUserId"
            };

            // Arrange 
            DemoteUserFromRoleCommandService sut =
                new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await sut.ExecuteAsync(command));
        }

        [Test]
        public void ThrowsInvalidOperationExceptionWhenTheUserTriesToDemoteHimself()
        {
            const string userId = "UserId";

            var usersStub = new[]
            {
                new User() { Id = userId }
            }
            .AsQueryable()
            .BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub.Setup(r => r.All(It.IsAny<bool>()))
                .Returns(usersStub.Object);

            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            DemoteUserFromRole command = new DemoteUserFromRole()
            {
                UserId = userId,
                LoggedUserId = userId
            };

            // Arrange 
            DemoteUserFromRoleCommandService sut =
                new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await sut.ExecuteAsync(command));
        }

        [Test]
        public void ThrowsInvalidOperationExceptionWhenTheUserTriesToDemoteUserWithAdministratorRoleFromAnotherRole()
        {
            const string userId = "UserId";

            var usersStub = new[]
            {
                new User()
                {
                    Id = userId,
                    Roles = new List<Role>()
                    {
                        new Role() { Name = "Administrator"}
                    }
                }
            }
            .AsQueryable()
            .BuildMock();

            var userRepositoryStub = new Mock<IEfRepository<User>>();
            userRepositoryStub.Setup(r => r.All(It.IsAny<bool>()))
                .Returns(usersStub.Object);

            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            DemoteUserFromRole command = new DemoteUserFromRole()
            {
                UserId = userId,
                LoggedUserId = "AnotherUserId",
                RoleName = RoleType.Uploader.ToString()
            };

            // Arrange 
            DemoteUserFromRoleCommandService sut =
                new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Act && Assert
            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await sut.ExecuteAsync(command));
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

            DemoteUserFromRole command = new DemoteUserFromRole()
            {
                UserId = userId,
                LoggedUserId = "AnotherUserId",
                RoleName = "AnotherRoleName"
            };

            // Arrange 
            DemoteUserFromRoleCommandService sut =
                new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Act && Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(
                async () => await sut.ExecuteAsync(command));

            StringAssert.Contains("does not exists", ex.Message);
        }

        [Test]
        public async Task RemoveRoleFromUserRolesWhenCommandContainsValidData()
        {
            const string roleName = "RoleName";
            const string userId = "UserId";

            List<Role> userRoles = new List<Role>()
            {
                new Role() { Name = roleName }
            };

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

            DemoteUserFromRole command = new DemoteUserFromRole()
            {
                UserId = userId,
                LoggedUserId = "AnotherUserId",
                RoleName = roleName
            };

            // Arrange 
            DemoteUserFromRoleCommandService sut =
                new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Act
            await sut.ExecuteAsync(command);

            // Assert
            Assert.AreEqual(0, userRoles.Count);
        }
    }
}
