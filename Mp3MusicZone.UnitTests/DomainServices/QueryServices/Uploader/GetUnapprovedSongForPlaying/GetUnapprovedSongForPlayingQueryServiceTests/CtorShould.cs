﻿namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.Uploader
    .GetUnapprovedSongForPlaying.GetUnapprovedSongForPlayingQueryServiceTests
{
    using Moq;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongForPlaying;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedSongProviderIsNull()
        {
            var songRepositoryStub = new Mock<IEfRepository<Song>>();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetUnapprovedSongForPlayingQueryService(
                    songProvider: null,
                    songRepository: songRepositoryStub.Object));
        }

        [Test]
        public void SavePassedSongProviderWhenIsNotNull()
        {
            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            var songProviderStub = new Mock<ISongProvider>();

            // Arrange && Act
            GetUnapprovedSongForPlayingQueryService sut =
                new GetUnapprovedSongForPlayingQueryService(
                    songProvider: songProviderStub.Object,
                    songRepository: songRepositoryStub.Object);

            // Assert
            var actualSongProvider = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(ISongProvider))
                .GetValue(sut);

            Assert.AreSame(songProviderStub.Object, actualSongProvider);
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenPassedSongRepositoryIsNull()
        {
            var songProviderStub = new Mock<ISongProvider>();

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetUnapprovedSongForPlayingQueryService(
                    songProvider: songProviderStub.Object,
                    songRepository: null));
        }

        [Test]
        public void SavePassedSongRepositoryWhenIsNotNull()
        {
            var songRepositoryStub = new Mock<IEfRepository<Song>>();
            var songProviderStub = new Mock<ISongProvider>();

            // Arrange && Act
            GetUnapprovedSongForPlayingQueryService sut =
                new GetUnapprovedSongForPlayingQueryService(
                    songProvider: songProviderStub.Object,
                    songRepository: songRepositoryStub.Object);

            // Assert
            var actualSongProvider = sut.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(fi => fi.FieldType == typeof(IEfRepository<Song>))
                .GetValue(sut);

            Assert.AreSame(songRepositoryStub.Object, actualSongProvider);
        }
    }
}
