namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetLastApproved.GetLastApprovedSongsQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetLastApproved;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedSongRepositoryIsNull()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetLastApprovedSongsQueryService(
                    songRepository: null));
        }

        [Test]
        public void SavePassedSongRepositoryWhenIsNotNull()
        { 
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act
            GetLastApprovedSongsQueryService sut = new GetLastApprovedSongsQueryService(
                songRepository: songRepositoryStub.Object);

            // Assert
            var actualSongRepository = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<Song>))
                .GetValue(sut);

            Assert.AreSame(songRepositoryStub.Object, actualSongRepository);
        }
    }
}
