namespace Mp3MusicZone.Web.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.Contracts;
    using DomainServices.QueryServices.Songs.GetLastApproved;
    using EfDataAccess;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Mp3MusicZone.DomainServices;
    using Mp3MusicZone.EfDataAccess.EfRepositories;
    using Mp3MusicZone.Web.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using ViewModels.Songs;
    using Web.ViewModels;

    using static Common.Constants.WebConstants;

    public class HomeController : Controller
    {
        private readonly IQueryService<GetLastApprovedSongs, IEnumerable<Song>> getSongs;

        public HomeController(
            IQueryService<GetLastApprovedSongs, IEnumerable<Song>> getSongs)
        {
            if (getSongs is null)
                throw new ArgumentNullException(nameof(getSongs));
            
            this.getSongs = getSongs;
        }

        public async Task<IActionResult> Index([FromServices]IConfiguration conf,
            [FromServices]MusicZoneDbContext context)
        {
            // sample admin logging
            //var logger = LogManager.GetLogger("AdminLogger");
            //logger.Trace("asd");

            GetLastApprovedSongs query = new GetLastApprovedSongs()
            {
                Count = DefaultHomePageLastApprovedSongsCount
            };

            IEnumerable<Song> lastSongs = await this.getSongs.ExecuteAsync(query);

            IEnumerable<SongListingViewModel> model =
                Mapper.Map<IEnumerable<SongListingViewModel>>(lastSongs);

            return View(model);
        }

        public IActionResult About()
        {
            // Testing loggin
            //ViewData["Message"] = "Your application description page.";
            //throw new ArgumentException("Testing NLog db");
            
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
