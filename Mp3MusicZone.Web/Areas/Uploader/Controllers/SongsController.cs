﻿namespace Mp3MusicZone.Web.Areas.Uploader.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.CommandServices.Uploader.ApproveSong;
    using DomainServices.CommandServices.Uploader.RejectSong;
    using DomainServices.Contracts;
    using DomainServices.QueryServices.Uploader.GetUnapprovedSongs;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.Controllers;
    using Web.ViewModels.Songs;

    [Authorize]
    [Area("Uploader")]
    public class SongsController : Controller
    {
        private readonly ICommandService<ApproveSong> approveSong;
        private readonly ICommandService<RejectSong> rejectSong;

        private readonly IQueryService<GetUnapprovedSongs, IEnumerable<Song>> getUnapprovedSongs;

        public SongsController(
            ICommandService<ApproveSong> approveSong,
            ICommandService<RejectSong> rejectSong,
            IQueryService<GetUnapprovedSongs, IEnumerable<Song>> getUnapprovedSongs)
        {
            if (approveSong is null)
                throw new ArgumentNullException(nameof(approveSong));

            if (rejectSong is null)
                throw new ArgumentNullException(nameof(rejectSong));

            if (getUnapprovedSongs is null)
                throw new ArgumentNullException(nameof(getUnapprovedSongs));

            this.approveSong = approveSong;
            this.rejectSong = rejectSong;
            this.getUnapprovedSongs = getUnapprovedSongs;
        }

        public async Task<IActionResult> UnapprovedSongs()
        {
            IEnumerable<Song> unapprovedSongs = null;
            GetUnapprovedSongs query = new GetUnapprovedSongs();

            string message = await this.CallServiceAsync(
                async () => unapprovedSongs = await this.getUnapprovedSongs.ExecuteAsync(query));

            if (message != null)
            {
                return RedirectToAction(
                    nameof(HomeController.Index), "Home", new { area = "" })
                    .WithErrorMessage(message);
            }

            IEnumerable<SongListingViewModel> model =
                Mapper.Map<IEnumerable<SongListingViewModel>>(unapprovedSongs);

            return View(model);
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
                if (message.Contains("do not have permissions"))
                {
                    return RedirectToAction(
                        nameof(HomeController.Index), "Home", new { area = "" })
                        .WithErrorMessage(message);
                }

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
                if (message.Contains("do not have permissions"))
                {
                    return RedirectToAction(
                        nameof(HomeController.Index), "Home",new { area = "" })
                        .WithErrorMessage(message);
                }

                return RedirectToAction(nameof(UnapprovedSongs))
                    .WithErrorMessage(message);
            }

            return RedirectToAction(nameof(UnapprovedSongs))
                .WithSuccessMessage("Song rejected successfully.");
        }
    }
}