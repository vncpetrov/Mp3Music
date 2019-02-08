namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.
    Songs.GetForDeleteById.GetSongForDeleteByIdQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetForDeleteById;
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
                () => new GetSongForDeleteByIdQueryService(
                    songRepository: null));
        }

        [Test]
        public void SavePassedSongRepositoryWhenIsNotNull()
        {
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act
            GetSongForDeleteByIdQueryService sut =
                new GetSongForDeleteByIdQueryService(
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
