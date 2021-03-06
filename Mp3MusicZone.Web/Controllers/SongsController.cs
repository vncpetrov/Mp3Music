﻿namespace Mp3MusicZone.Web.Controllers
{
    using AutoMapper;
    using Components;
    using Domain.Models;
    using DomainServices.CommandServices.Songs.DeleteSong;
    using DomainServices.CommandServices.Songs.EditSong;
    using DomainServices.CommandServices.Songs.UploadSong;
    using DomainServices.Contracts;
    using DomainServices.QueryServices;
    using DomainServices.QueryServices.Songs.GetApprovedSongsByUser;
    using DomainServices.QueryServices.Songs.GetForDeleteById;
    using DomainServices.QueryServices.Songs.GetForEditById;
    using DomainServices.QueryServices.Songs.GetSongForPlaying;
    using DomainServices.QueryServices.Songs.GetSongs;
    using DomainServices.QueryServices.Songs.GetSongsCount;
    using FacadeServices;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ViewModels;
    using ViewModels.Songs;
    using Web.ViewModels.Shared;

    using static Common.Constants.ModelConstants;
    using static Common.Constants.WebConstants;

    [Authorize]
    public class SongsController : Controller
    {
        private readonly ICommandService<EditSong> editSong;
        private readonly ICommandService<UploadSong> uploadSong;
        private readonly ICommandService<DeleteSong> deleteSong;

        private readonly IQueryService<GetSongForEditById, Song> getSongForEdit;
        private readonly IQueryService<GetSongForDeleteById, Song> getSongForDelete;
        private readonly IQueryService<GetSongsCount, int> getSongsCount;
        private readonly IQueryService<GetSongs, IEnumerable<Song>> getSongs;
        private readonly IQueryService<GetApprovedSongsByUser, IEnumerable<Song>> getSongsByUser;
        
        private readonly ISongPlayer songPlayer;

        public SongsController(
            ICommandService<EditSong> editSong,
            ICommandService<UploadSong> uploadSong,
            ICommandService<DeleteSong> deleteSong,

            IQueryService<GetSongForEditById, Song> getSongForEdit,
            IQueryService<GetSongForDeleteById, Song> getSongForDelete,
            IQueryService<GetSongsCount, int> getSongsCount,
            IQueryService<GetSongs, IEnumerable<Song>> getSongs,
            IQueryService<GetApprovedSongsByUser, IEnumerable<Song>> getSongsByUser,

            ISongPlayer songPlayer)
        {
            if (editSong is null)
                throw new ArgumentNullException(nameof(editSong));

            if (uploadSong is null)
                throw new ArgumentNullException(nameof(uploadSong));

            if (deleteSong is null)
                throw new ArgumentNullException(nameof(deleteSong));


            if (getSongForEdit is null)
                throw new ArgumentException(nameof(getSongForEdit));

            if (getSongForDelete is null)
                throw new ArgumentException(nameof(getSongForDelete));

            if (getSongsCount is null)
                throw new ArgumentException(nameof(getSongsCount));

            if (getSongs is null)
                throw new ArgumentException(nameof(getSongs));

            if (getSongsByUser is null)
                throw new ArgumentException(nameof(getSongsByUser));

            if (songPlayer is null)
                throw new ArgumentException(nameof(songPlayer));

            this.editSong = editSong;
            this.uploadSong = uploadSong;
            this.deleteSong = deleteSong;

            this.getSongForEdit = getSongForEdit;
            this.getSongForDelete = getSongForDelete;
            this.getSongsCount = getSongsCount;
            this.getSongs = getSongs;
            this.getSongsByUser = getSongsByUser;

            this.songPlayer = songPlayer;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int page = 1, string searchTerm = null)
        {
            IEnumerable<Song> songs = null;

            GetSongs songsQuery = new GetSongs()
            {
                PageInfo = new PageInfo(page, DefaultPageSize),
                SearchInfo = new SearchInfo(searchTerm)
            };

            string message = await this.CallServiceAsync(
                async () => songs = await this.getSongs.ExecuteAsync(songsQuery));

            if (message != null)
            {
                return RedirectToAction(
                    nameof(HomeController.Index), "Home", new { area = "" })
                    .WithErrorMessage(message);
            }

            GetSongsCount songsCountQuery = new GetSongsCount()
            {
                Approved = true,
                SearchInfo = new SearchInfo(searchTerm)
            };

            int songsCount = await this.getSongsCount.ExecuteAsync(songsCountQuery);

            IEnumerable<SongListingViewModel> songsModel =
                Mapper.Map<IEnumerable<SongListingViewModel>>(songs);

            SearchViewModel<PaginatedViewModel<SongListingViewModel>> model =
                ViewModelFactory.CreateSearchPaginatedViewModel<SongListingViewModel>(
                    songsModel,
                    page,
                    DefaultPageSize,
                    songsCount,
                    searchTerm,
                    "songs");

            return View(model);
        }

        [AjaxOnly]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "userId" })]
        public async Task<IActionResult> UserUploadedSongsAjax(string userId)
        {
            IEnumerable<Song> songs = null;

            GetApprovedSongsByUser query = new GetApprovedSongsByUser()
            {
                UserId = userId
            };

            string message = await this.CallServiceAsync(
                async () => songs = await this.getSongsByUser.ExecuteAsync(query));

            IEnumerable<UserSongListingViewModel> model = Mapper.Map<IEnumerable<UserSongListingViewModel>>(songs);

            return PartialView("_UserSongListing", model);
        }

        [AjaxOnly]
        [AllowAnonymous]
        public async Task<IActionResult> FilteredSongsAjax(string searchTerm)
        {
            IEnumerable<Song> songs = null;

            GetSongs songsQuery = new GetSongs()
            {
                PageInfo = new PageInfo(1, DefaultPageSize),
                SearchInfo = new SearchInfo(searchTerm)
            };

            string message = await this.CallServiceAsync(
                async () => songs = await this.getSongs.ExecuteAsync(songsQuery));

            IEnumerable<SongListingViewModel> model =
                Mapper.Map<IEnumerable<SongListingViewModel>>(songs);

            return PartialView("_SongListing", model);
        }

        [AjaxOnly]
        [AllowAnonymous]
        public async Task<IActionResult> PaginationAjax(string searchTerm)
        {
            int songsCount = await this.getSongsCount.ExecuteAsync(new GetSongsCount()
            {
                Approved = true,
                SearchInfo = new SearchInfo(searchTerm)
            });

            return ViewComponent(
                typeof(PaginationComponent),
                new
                {
                    pageInfo = new PaginatedViewModel<string>(
                        null, 1, DefaultPageSize, songsCount),
                    searchTerm = searchTerm,
                    actionToCall = "all"
                });
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

            string fileExtension = model.File
                .GetFileExtension();

            UploadSong command = new UploadSong()
            {
                UploaderId = this.User?.FindFirst(ClaimTypes.NameIdentifier).Value,
                Title = model.Title,
                FileExtension = fileExtension,
                ReleasedYear = model.ReleasedYear,
                Singer = model.Singer,
                SongFile = model.File.ToByteArray()
            };

            string message = await this.CallServiceAsync(
                async () => await this.uploadSong.ExecuteAsync(command));

            if (message != null)
            {
                return View(model)
                    .WithErrorMessage(message);
            }
            
            return View()
                .WithSuccessMessage("Song uploaded successfully.");
        }

        public async Task<IActionResult> Edit(string id)
        {
            Song song = null;
            GetSongForEditById query = new GetSongForEditById()
            {
                SongId = id
            };

            string message = await this.CallServiceAsync(
                async () => song = await this.getSongForEdit.ExecuteAsync(query));

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
        public async Task<IActionResult> Edit(string id, SongFormModel model)
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
                return View()
                    .WithErrorMessage(message);
            }

            return View()
                .WithSuccessMessage("Song edited successfully.");
        }

        public async Task<IActionResult> Delete(string id, string returnUrl = null)
        {
            Song song = null;
            GetSongForDeleteById query = new GetSongForDeleteById()
            {
                SongId = id
            };

            string message = await this.CallServiceAsync(
               async () => song = await this.getSongForDelete.ExecuteAsync(query));

            if (message != null)
            {
                return View()
                    .WithErrorMessage(message);
            }

            DeleteSongViewModel model = Mapper.Map<DeleteSongViewModel>(song);
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string id, string returnUrl = null)
        {
            DeleteSong command = new DeleteSong()
            {
                SongId = id
            };

            string message = await this.CallServiceAsync(
                async () => await this.deleteSong.ExecuteAsync(command));

            if (message != null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                               .WithErrorMessage(message);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home")
                .WithSuccessMessage("Song deleted successfully.");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Play(string id)
        {
            SongForPlayingDTO song = null;

            string message = await this.CallServiceAsync(
                async () => song = await this.songPlayer.GetSongAsync(id));

            if (message != null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage(message);
            }

            MemoryStream ms = new MemoryStream(song.File);

            return File(ms, $"audio/{song.FileExtension}", song.HeadingText);
        }
    }
}
