namespace Mp3MusicZone.Web.Areas.Uploader.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.CommandServices.Uploader.ApproveSong;
    using DomainServices.CommandServices.Uploader.RejectSong;
    using DomainServices.Contracts;
    using DomainServices.QueryServices.Songs.GetSongsCount;
    using DomainServices.QueryServices.Uploader.GetUnapprovedSongs;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Mp3MusicZone.DomainServices.QueryServices;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongForPlaying;
    using Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongForPlaying;
    using Mp3MusicZone.Web.FacadeServices;
    using Mp3MusicZone.Web.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Web.Controllers;
    using Web.ViewModels.Shared;
    using Web.ViewModels.Songs;

    using static Common.Constants.WebConstants;

    [Authorize]
    [Area("Uploader")]
    public class SongsController : Controller
    {
        private readonly ICommandService<ApproveSong> approveSong;
        private readonly ICommandService<RejectSong> rejectSong;

        private readonly IQueryService<GetUnapprovedSongs, IEnumerable<Song>> getUnapprovedSongs;
        private readonly IQueryService<GetSongsCount, int> getSongsCount;
        IQueryService<GetUnapprovedSongForPlaying, UnapprovedSongForPlayingDTO> getUnapprovedSong;
        
        public SongsController(
            ICommandService<ApproveSong> approveSong,
            ICommandService<RejectSong> rejectSong,

            IQueryService<GetUnapprovedSongs, IEnumerable<Song>> getUnapprovedSongs,
            IQueryService<GetSongsCount, int> getSongsCount,
            IQueryService<GetUnapprovedSongForPlaying, UnapprovedSongForPlayingDTO> getUnapprovedSong)
        {
            if (approveSong is null)
                throw new ArgumentNullException(nameof(approveSong));

            if (rejectSong is null)
                throw new ArgumentNullException(nameof(rejectSong));

            if (getUnapprovedSongs is null)
                throw new ArgumentNullException(nameof(getUnapprovedSongs));

            if (getSongsCount is null)
                throw new ArgumentNullException(nameof(getSongsCount));

            if (getUnapprovedSong is null)
                throw new ArgumentNullException(nameof(getUnapprovedSong));
            
            this.approveSong = approveSong;
            this.rejectSong = rejectSong;
            this.getUnapprovedSongs = getUnapprovedSongs;
            this.getSongsCount = getSongsCount;
            this.getUnapprovedSong = getUnapprovedSong;
        }

        public async Task<IActionResult> UnapprovedSongs(int page = 1)
        {
            GetUnapprovedSongs getSongsQuery = new GetUnapprovedSongs()
            {
                Page = page
            };

            IEnumerable<Song> unapprovedSongs = await this.getUnapprovedSongs.ExecuteAsync(getSongsQuery);

            GetSongsCount getSongsCountQuery = new GetSongsCount()
            {
                Approved = false,
                SearchInfo = new SearchInfo(null)
            };

            int songsCount = await this.getSongsCount.ExecuteAsync(getSongsCountQuery);

            IEnumerable<SongListingViewModel> songsModel =
                Mapper.Map<IEnumerable<SongListingViewModel>>(unapprovedSongs);

            PaginatedViewModel<SongListingViewModel> model =
                ViewModelFactory.CreatePaginatedViewModel<SongListingViewModel>(
                    songsModel,
                    page,
                    DefaultPageSize,
                    songsCount);

            return View(model);
        }

        public async Task<IActionResult> PlayUnapproved(string id)
        {
            UnapprovedSongForPlayingDTO song = null;
            GetUnapprovedSongForPlaying query = new GetUnapprovedSongForPlaying()
            {
                SongId = id
            };

            string message = await this.CallServiceAsync(
                async () => song = await this.getUnapprovedSong.ExecuteAsync(query));

            if (message != null)
            {
                return View()
                    .WithErrorMessage(message);
            }

            MemoryStream ms = new MemoryStream(song.File);

            return File(ms, $"audio/{song.FileExtension}");
        }

        public async Task<IActionResult> Approve(string id)
        {
            ApproveSong command = new ApproveSong()
            {
                SongId = id
            };

            string message = await this.CallServiceAsync(
                async () => await this.approveSong.ExecuteAsync(command));

            if (message != null)
            {
                return RedirectToAction(nameof(UnapprovedSongs))
                    .WithErrorMessage(message);
            }

            return RedirectToAction(nameof(UnapprovedSongs))
                .WithSuccessMessage("Song approved successfully.");
        }

        public async Task<IActionResult> Reject(string id)
        {
            RejectSong command = new RejectSong()
            {
                SongId = id
            };

            string message = await this.CallServiceAsync(
                async () => await this.rejectSong.ExecuteAsync(command));

            if (message != null)
            {
                return RedirectToAction(nameof(UnapprovedSongs))
                    .WithErrorMessage(message);
            }

            return RedirectToAction(nameof(UnapprovedSongs))
                .WithSuccessMessage("Song rejected successfully.");
        }
    }
}
