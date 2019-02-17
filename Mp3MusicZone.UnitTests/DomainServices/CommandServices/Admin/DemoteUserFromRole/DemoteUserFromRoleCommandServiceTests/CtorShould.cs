namespace Mp3MusicZone.UnitTests.DomainServices.CommandServices.
    Admin.DemoteUserFromRole.DemoteUserFromRoleCommandServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.CommandServices.Admin.DemoteUserFromRole;
    using Mp3MusicZone.EfDataAccess;
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
            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new DemoteUserFromRoleCommandService(
                    userRepository: null,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object));
        }

        [Test]
        public void SavePassedUserRepositoryWhenIsNotNull()
        {
            var userRepositoryStub = new Mock<IEfRepository<User>>();
            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            // Arrange && Act
            DemoteUserFromRoleCommandService sut =
                new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Assert
            var actualUserRepository = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<User>))
                .GetValue(sut);

            Assert.AreSame(userRepositoryStub.Object, actualUserRepository);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedRoleRepositoryIsNull()
        {
            var userRepositoryStub = new Mock<IEfRepository<User>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: null,
                    contextSaveChanges: contextSaveChangesStub.Object));
        }

        [Test]
        public void SavePassedRoleRepositoryWhenIsNotNull()
        {
            var userRepositoryStub = new Mock<IEfRepository<User>>();
            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            // Arrange && Act
            DemoteUserFromRoleCommandService sut =
                new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Assert
            var actualRoleRepository = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<Role>))
                .GetValue(sut);

            Assert.AreSame(roleRepositoryStub.Object, actualRoleRepository);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedContextSaveChangesIsNull()
        {
            var userRepositoryStub = new Mock<IEfRepository<User>>();
            var roleRepositoryStub = new Mock<IEfRepository<Role>>(); 

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: null));
        }

        [Test]
        public void SavePassedContextSaveChangesWhenIsNotNull()
        {
            var userRepositoryStub = new Mock<IEfRepository<User>>();
            var roleRepositoryStub = new Mock<IEfRepository<Role>>();
            var contextSaveChangesStub = new Mock<MusicZoneDbContext>(
                new[] { "Fake Connection string" });

            // Arrange && Act
            DemoteUserFromRoleCommandService sut =
                new DemoteUserFromRoleCommandService(
                    userRepository: userRepositoryStub.Object,
                    roleRepository: roleRepositoryStub.Object,
                    contextSaveChanges: contextSaveChangesStub.Object);

            // Assert
            var actualContextSaveChanges = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfDbContextSaveChanges))
                .GetValue(sut);

            Assert.AreSame(contextSaveChangesStub.Object, actualContextSaveChanges);
        } 
    }
}
