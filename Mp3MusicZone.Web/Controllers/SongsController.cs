namespace Mp3MusicZone.Web.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.CommandServices.Songs.EditSong;
    using DomainServices.CommandServices.Songs.UploadSong;
    using DomainServices.Contracts;
    using DomainServices.QueryServices.Songs.GetById;
    using DomainServices.QueryServices.Songs.GetSongForPlaying;
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
        private readonly ICommandService<EditSong> editSong;
        private readonly ICommandService<UploadSong> uploadSong;

        private readonly IQueryService<GetSongById, Song> getSong;
        private readonly IQueryService<GetSongForPlaying, SongForPlayingDTO> getSongForPlaying;
        
        public SongsController(
            ICommandService<EditSong> editSong,
            ICommandService<UploadSong> uploadSong,
            IQueryService<GetSongById, Song> getSong,
            IQueryService<GetSongForPlaying, SongForPlayingDTO> getSongForPlaying)
        {
            if (editSong is null)
                throw new ArgumentNullException(nameof(editSong));

            if (uploadSong is null)
                throw new ArgumentNullException(nameof(uploadSong));

            if (getSong is null)
                throw new ArgumentException(nameof(getSong));

            if (getSongForPlaying is null)
                throw new ArgumentException(nameof(getSongForPlaying));

            this.editSong = editSong;
            this.uploadSong = uploadSong;

            this.getSong = getSong;
            this.getSongForPlaying = getSongForPlaying;
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


                await this.uploadSong.ExecuteAsync(command);
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
            
            GetSongById query = new GetSongById()
            {
                SongId = id
            };

            string message = await this.CallServiceAsync(
                async () => song = await this.getSong.ExecuteAsync(query));

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

            string message = await this.CallServiceAsync(
                async () => await this.editSong.ExecuteAsync(command));

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
            GetSongForPlaying query = new GetSongForPlaying()
            {
                SongId = id
            };

            SongForPlayingDTO song = null;

            string message = await this.CallServiceAsync(
                async () => song = await this.getSongForPlaying.ExecuteAsync(query));

            if (message != null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage(message);
            }

            MemoryStream ms = new MemoryStream(song.File);

            return File(ms, $"audio/{song.FileExtension}", song.HeadingText);
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
            catch (Exception ex)
            {
                message = "We're sorry, something went wrong. Please try again later.";
            }

            return message;
        }
    }
}
