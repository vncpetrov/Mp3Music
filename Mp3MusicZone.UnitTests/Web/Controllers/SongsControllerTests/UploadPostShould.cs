namespace Mp3MusicZone.UnitTests.Web.Controllers.SongsControllerTests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices.CommandServices.Songs.DeleteSong;
    using Mp3MusicZone.DomainServices.CommandServices.Songs.EditSong;
    using Mp3MusicZone.DomainServices.CommandServices.Songs.UploadSong;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetApprovedSongsByUser;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetForDeleteById;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetForEditById;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongs;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongsCount;
    using Mp3MusicZone.Web.Controllers;
    using Mp3MusicZone.Web.FacadeServices;
    using Mp3MusicZone.Web.Infrastructure;
    using Mp3MusicZone.Web.Infrastructure.Filters;
    using Mp3MusicZone.Web.ViewModels.Songs;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static Mp3MusicZone.UnitTests.Utils.AttributesUtils;
    using static Mp3MusicZone.Common.Constants.ModelConstants;
    using Mp3MusicZone.Domain.Exceptions;

    [TestFixture]
    public class UploadPostShould
    {
        [Test]
        public async Task ReturnViewWithErrorMessageWhenSongFileIsNull()
        {
            SongFormModel model = new SongFormModel();

            // Arrange
            SongsController sut = CreateSongsController(Mock.Of<ICommandService<UploadSong>>());

            // Act
            var result = await sut.Upload(model) as WithMessageResult;

            // Assert
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("danger", result.Type);
            StringAssert.Contains("choose a file", result.Message);
        }

        [Test]
        public async Task ReturnViewWithTheSameModelWhenSongFileIsNull()
        {
            SongFormModel model = new SongFormModel();

            // Arrange
            SongsController sut = CreateSongsController(Mock.Of<ICommandService<UploadSong>>());

            // Act
            var result = await sut.Upload(model) as WithMessageResult;
            var realAction = result.Action as ViewResult;

            // Assert  
            Assert.IsNotNull(realAction.Model);
            Assert.AreSame(model, realAction.Model);
        }

        [Test]
        public async Task ReturnViewWithErrorMessageWhenPassedFileTypeIsNotSong()
        {
            var fileStub = new Mock<IFormFile>();
            fileStub.Setup(f => f.ContentType).Returns(() => "InvalidSongFile");

            SongFormModel model = new SongFormModel()
            {
                File = fileStub.Object
            };

            // Arrange
            SongsController sut = CreateSongsController(Mock.Of<ICommandService<UploadSong>>());

            // Act
            var result = await sut.Upload(model) as WithMessageResult;

            // Assert
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("danger", result.Type);
            StringAssert.Contains("should be an audio", result.Message);
        }

        [Test]
        public async Task ReturnViewWithErrorMessageWhenPassedFileIsTooLarge()
        {
            var fileStub = new Mock<IFormFile>();
            fileStub.Setup(f => f.ContentType).Returns(() => "audio");
            fileStub.Setup(f => f.Length).Returns(() => SongMaxLength + 1);

            SongFormModel model = new SongFormModel()
            {
                File = fileStub.Object
            };

            // Arrange
            SongsController sut = CreateSongsController(Mock.Of<ICommandService<UploadSong>>());

            // Act
            var result = await sut.Upload(model) as WithMessageResult;

            // Assert
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("danger", result.Type);
            StringAssert.Contains("no more than", result.Message);
        }

        [Test]
        public async Task ReturnViewWithErrorMessageWhenCommandServiceThrowNotFoundException()
        {
            string expectedMessage = "Fake message";

            var fileStub = new Mock<IFormFile>();
            fileStub.Setup(f => f.ContentType).Returns(() => "audio");
            fileStub.Setup(f => f.Length).Returns(() => SongMaxLength - 1);

            var uploadSongStub = new Mock<ICommandService<UploadSong>>();
            uploadSongStub.Setup(u => u.ExecuteAsync(It.IsAny<UploadSong>()))
                .Throws(new NotFoundException(expectedMessage));

            SongFormModel model = new SongFormModel()
            {
                File = fileStub.Object
            };

            // Arrange
            SongsController sut = CreateSongsController(
                uploadSong: uploadSongStub.Object);

            // Act
            var result = await sut.Upload(model) as WithMessageResult;

            // Assert
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("danger", result.Type);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [Test]
        public async Task ReturnViewWithSuccessMessageWhenSongIsUploadedSuccessfully()
        {
            string expectedMessage = "successfully";

            var fileStub = new Mock<IFormFile>();
            fileStub.Setup(f => f.ContentType).Returns(() => "audio");
            fileStub.Setup(f => f.Length).Returns(() => SongMaxLength - 1);

            var uploadSongStub = new Mock<ICommandService<UploadSong>>();

            SongFormModel model = new SongFormModel()
            {
                File = fileStub.Object
            };

            // Arrange
            SongsController sut = CreateSongsController(
                uploadSong: uploadSongStub.Object);

            // Act
            var result = await sut.Upload(model) as WithMessageResult;

            // Assert
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("success", result.Type);
            StringAssert.Contains(expectedMessage, result.Message);
        }

        [Test]
        public async Task ReturnViewWithoutModelWhenSongIsUploadedSuccessfully()
        {
            string expectedMessage = "successfully";

            var fileStub = new Mock<IFormFile>();
            fileStub.Setup(f => f.ContentType).Returns(() => "audio");
            fileStub.Setup(f => f.Length).Returns(() => SongMaxLength - 1);

            var uploadSongStub = new Mock<ICommandService<UploadSong>>();

            SongFormModel model = new SongFormModel()
            {
                File = fileStub.Object
            };

            // Arrange
            SongsController sut = CreateSongsController(
                uploadSong: uploadSongStub.Object);

            // Act
            var result = await sut.Upload(model) as WithMessageResult;
            var realAction = result.Action as ViewResult;

            // Assert
            Assert.IsNull(realAction.Model);
        }

        [TestCase(typeof(HttpPostAttribute))]
        [TestCase(typeof(ValidateModelStateAttribute))]
        public void HaveRequiredAttribute(Type attrType)
        {
            // Arrange
            SongsController sut = CreateSongsController(Mock.Of<ICommandService<UploadSong>>());

            // Act && Assert
            Assert.IsTrue(MethodHasAttribute(
                () => sut.Upload(default(SongFormModel)),
                attrType));
        }

        private static SongsController CreateSongsController(
            ICommandService<UploadSong> uploadSong)
            => new SongsController(
                    Mock.Of<ICommandService<EditSong>>(),
                    uploadSong,
                    Mock.Of<ICommandService<DeleteSong>>(),
                    Mock.Of<IQueryService<GetSongForEditById, Song>>(),
                    Mock.Of<IQueryService<GetSongForDeleteById, Song>>(),
                    Mock.Of<IQueryService<GetSongsCount, int>>(),
                    Mock.Of<IQueryService<GetSongs, IEnumerable<Song>>>(),
                    Mock.Of<IQueryService<GetApprovedSongsByUser, IEnumerable<Song>>>(),
                    Mock.Of<ISongPlayer>());
    }
}
