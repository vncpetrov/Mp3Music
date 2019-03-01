namespace Mp3MusicZone.UnitTests.Web.Controllers.SongsControllerTests
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Mp3MusicZone.Domain.Exceptions;
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
    using Mp3MusicZone.Web.Infrastructure.Mappings;
    using Mp3MusicZone.Web.ViewModels.Songs;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static Mp3MusicZone.Common.Constants.ModelConstants;
    using static Mp3MusicZone.UnitTests.Utils.AttributesUtils;

    [TestFixture]
    public class EditPostShould
    {
        public EditPostShould()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }

        [TestCase(typeof(HttpPostAttribute))]
        [TestCase(typeof(ValidateModelStateAttribute))]
        public void HaveRequiredAttribute(Type attrType)
        {
            // Arrange
            SongsController sut = CreateSongsController(Mock.Of<ICommandService<EditSong>>());

            // Act && Assert
            Assert.IsTrue(MethodHasAttribute(
                () => sut.Edit(It.IsAny<string>(), default(SongFormModel)),
                attrType));
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
            SongsController sut = CreateSongsController(
                editSong: Mock.Of<ICommandService<EditSong>>());

            // Act
            var result = await sut.Edit(It.IsAny<string>(), model) as WithMessageResult;

            // Assert
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("danger", result.Type);
            StringAssert.Contains("no more than", result.Message);
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
            SongsController sut = CreateSongsController(
                editSong: Mock.Of<ICommandService<EditSong>>());

            // Act
            var result = await sut.Edit(It.IsAny<string>(), model) as WithMessageResult;

            // Assert
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("danger", result.Type);
            StringAssert.Contains("should be an audio", result.Message);
        }

        [Test]
        public async Task ReturnViewWithErrorMessageWhenCommandServiceThrowNotFoundException()
        {
            string expectedMessage = "Fake message";

            var fileStub = new Mock<IFormFile>();
            fileStub.Setup(f => f.ContentType).Returns(() => "audio");
            fileStub.Setup(f => f.Length).Returns(() => SongMaxLength - 1);

            var editSongStub = new Mock<ICommandService<EditSong>>();
            editSongStub.Setup(u => u.ExecuteAsync(It.IsAny<EditSong>()))
                .Throws(new NotFoundException(expectedMessage));

            SongFormModel model = new SongFormModel()
            {
                File = fileStub.Object
            };

            // Arrange
            SongsController sut = CreateSongsController(
                editSong: editSongStub.Object);

            // Act
            var result = await sut.Edit(It.IsAny<string>(), model) as WithMessageResult;

            // Assert
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("danger", result.Type);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [Test]
        public async Task TheEditSongCommandServiceOnce()
        {
            var fileStub = new Mock<IFormFile>();
            fileStub.Setup(f => f.ContentType).Returns(() => "audio");
            fileStub.Setup(f => f.Length).Returns(() => SongMaxLength - 1);

            var editSongMock = new Mock<ICommandService<EditSong>>();

            SongFormModel model = new SongFormModel()
            {
                File = fileStub.Object
            };

            // Arrange
            SongsController sut = CreateSongsController(
                editSong: editSongMock.Object);

            // Act
            await sut.Edit(It.IsAny<string>(), model);

            // Assert
            editSongMock.Verify(x => x.ExecuteAsync(It.IsAny<EditSong>()), Times.Once());
        }

        [Test]
        public async Task ReturnViewWithSuccessMessageWhenSongIsEditedSuccessfully()
        {
            string expectedMessage = "edited successfully";

            var fileStub = new Mock<IFormFile>();
            fileStub.Setup(f => f.ContentType).Returns(() => "audio");
            fileStub.Setup(f => f.Length).Returns(() => SongMaxLength - 1);

            var editSongMock = new Mock<ICommandService<EditSong>>();

            SongFormModel model = new SongFormModel()
            {
                File = fileStub.Object
            };

            // Arrange
            SongsController sut = CreateSongsController(
                editSong: editSongMock.Object);

            // Act
            var result = await sut.Edit(It.IsAny<string>(), model) as WithMessageResult;

            // Assert          
            Assert.AreEqual(typeof(WithMessageResult), result.GetType());
            Assert.AreEqual("success", result.Type);
            StringAssert.Contains(expectedMessage, result.Message);
        }

        private static SongsController CreateSongsController(
            ICommandService<EditSong> editSong)
            => new SongsController(
                editSong,
                Mock.Of<ICommandService<UploadSong>>(),
                Mock.Of<ICommandService<DeleteSong>>(),
                Mock.Of<IQueryService<GetSongForEditById, Song>>(),
                Mock.Of<IQueryService<GetSongForDeleteById, Song>>(),
                Mock.Of<IQueryService<GetSongsCount, int>>(),
                Mock.Of<IQueryService<GetSongs, IEnumerable<Song>>>(),
                Mock.Of<IQueryService<GetApprovedSongsByUser, IEnumerable<Song>>>(),
                Mock.Of<ISongPlayer>());
    }
}
