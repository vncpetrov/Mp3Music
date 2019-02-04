namespace Mp3MusicZone.Web.Controllers
{
    using AutoMapper;
    using Domain.Contracts;
    using Domain.Models;
    using DomainServices.Contracts;
    using DomainServices.QueryServices.Songs.GetLastApproved;
    using EfDataAccess;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels.Songs;
    using Web.ViewModels;

    using static Common.Constants.WebConstants;

    public class HomeController : Controller
    {
        private readonly IQueryService<GetLastApprovedSongs, IEnumerable<Song>> getSongs;

        private readonly ICacheManager cacheManager;

        public HomeController(
            IQueryService<GetLastApprovedSongs, IEnumerable<Song>> getSongs,
            ICacheManager cacheManager)
        {
            if (getSongs is null)
                throw new ArgumentNullException(nameof(getSongs));

            this.getSongs = getSongs;
            this.cacheManager = cacheManager;
        }

        public async Task<IActionResult> Index()
        {
            GetLastApprovedSongs query = new GetLastApprovedSongs()
            {
                Count = DefaultHomePageLastApprovedSongsCount
            };

            IEnumerable<Song> songs = await this.getSongs.ExecuteAsync(query);

            IEnumerable<SongListingViewModel> model =
                Mapper.Map<IEnumerable<SongListingViewModel>>(songs);

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
        }
    }
}
