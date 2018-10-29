namespace Mp3MusicZone.Web.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.CommandServices.Songs.EditSong;
    using DomainServices.CommandServices.Songs.UploadSong;
    using DomainServices.Contracts;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ViewModels.Songs;

    using static Common.Constants.ModelConstants;

    [Authorize]
    public class SongsController : Controller
    {
        private readonly ICommandService<EditSong> editSongService;
        private readonly ICommandService<UploadSong> uploadSongService;
        private readonly ISongService songService;
        
        public SongsController(
            ICommandService<EditSong> editSongService,
            ICommandService<UploadSong> uploadSongService,
            ISongService songService)
        {
            if (editSongService is null)
                throw new ArgumentNullException(nameof(editSongService));

            if (uploadSongService is null)
                throw new ArgumentNullException(nameof(uploadSongService));

            if (songService is null)
                throw new ArgumentException(nameof(songService));

            this.editSongService = editSongService;
            this.uploadSongService = uploadSongService;
            this.songService = songService;
        }

        public IActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Upload(SongFormModel model)
        {
            if (model.File is null)
            {
                return View(model)
                    .WithErrorMessage("Please, choose a file!");
            }

            // Business validation?
            if (!model.File.IsSong() || model.File.Length > SongMaxLength)
            {
                return View(model)
                    .WithErrorMessage($"Your submission should be an audio file and no more than {SongMaxMBs} MBs in size!");
            }

            string message = await this.CallServiceAsync(async () =>
            {
                string fileExtension = model.File
                    .GetFileExtension();

                UploadSong command = new UploadSong()
                {
                    UploaderId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Title = model.Title,
                    FileExtension = fileExtension,
                    ReleasedYear = model.ReleasedYear,
                    Singer = model.Singer,
                    SongFile = model.File.ToByteArray()
                };


                await this.uploadSongService.ExecuteAsync(command);
            });

            if (message != null)
            {
                return View(model)
                    .WithErrorMessage(message);
            }

            //try
            //{
            //    string songExtension = model.File
            //        .GetFileExtension();

            //    await this.songService.UploadAsync(
            //         model.Title,
            //         songExtension,
            //         model.Singer,
            //         model.ReleasedYear,
            //         this.User.FindFirst(ClaimTypes.NameIdentifier).Value,
            //         model.File.ToByteArray());
            //}
            //catch (Exception ex)
            //{
            //    string message = ex.GetType() == typeof(InvalidOperationException) ?
            //        ex.Message :
            //        "We're sorry, something went wrong. Please try again later.";

            //    return View(model)
            //        .WithErrorMessage(message);
            //}

            return View()
                .WithSuccessMessage("Song uploaded successfully.");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Song song = null;

            //try
            //{
            //    song = this.songService.GetById(id);
            //}
            //catch (Exception ex)
            //{
            //    string message = ex.GetType() == typeof(InvalidOperationException) ?
            //        ex.Message :
            //        "We're sorry, something went wrong. Please try again later.";

            //    return View()
            //        .WithErrorMessage(message);
            //}

            string message = await this.CallServiceAsync(
                async () => song = await this.songService.GetByIdAsync(id));

            if (message != null)
            {
                return View()
                    .WithErrorMessage(message);
            }

            SongFormModel model = Mapper.Map<SongFormModel>(song);
            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Edit(int id, SongFormModel model)
        {
            if (model.File != null
                && (!model.File.IsSong() || model.File.Length > SongMaxLength))
            {
                return View(model)
                    .WithErrorMessage($"Your submission should be an audio file and no more than {SongMaxMBs} MBs in size!");
            }

            string message = await this.CallServiceAsync(async () =>
            {
                string fileExtension = model.File
                    .GetFileExtension();

                EditSong command = new EditSong()
                {
                    Title = model.Title,
                    FileExtension = fileExtension,
                    ReleasedYear = model.ReleasedYear,
                    Singer = model.Singer,
                    SongFile = model.File?.ToByteArray(),
                    SongId = id
                };

                await this.editSongService.ExecuteAsync(command);
            });

            if (message != null)
            {
                return View(model)
                    .WithErrorMessage(message);
            }

            //try
            //{
            //    string songExtension = model.File
            //        .GetFileExtension();

            //    await this.songService.EditAsync(
            //        id,
            //        model.Title,
            //        songExtension,
            //        model.Singer,
            //        model.ReleasedYear,
            //        model.File?.ToByteArray());
            //}
            //catch (Exception ex)
            //{
            //    string message = ex.GetType() == typeof(InvalidOperationException) ?
            //        ex.Message :
            //        "We're sorry, something went wrong. Please try again later.";

            //    return View(model)
            //        .WithErrorMessage(message);
            //}

            return View()
                .WithSuccessMessage("Song edited successfully.");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Play(int id)
        {
            Song song = null;

            string message = await this.CallServiceAsync(
                async () => song = await this.songService.GetByIdAsync(id));

            if (message != null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage(message);
            }

            byte[] songFile = await this.songService.GetSongFileAsync(song);

            MemoryStream ms = new MemoryStream(songFile);

            return File(ms, $"audio/{song.FileExtension}", $"{song.Singer} - {song.Title}, {song.ReleasedYear}.{song.FileExtension}");
        }

        private string CallService(Action action)
        {
            string message = null;

            try
            {
                action();
            }
            catch (InvalidOperationException ex)
            {
                message = ex.Message;
            }
            catch (Exception)
            {
                message = "We're sorry, something went wrong. Please try again later.";
            }

            return message;
        }

        private async Task<string> CallServiceAsync(Func<Task> func)
        {
            string message = null;

            try
            {
                await func();
            }
            catch (InvalidOperationException ex)
            {
                message = ex.Message;
            }
            catch (Exception)
            {
                message = "We're sorry, something went wrong. Please try again later.";
            }

            return message;
        }
    }
}
