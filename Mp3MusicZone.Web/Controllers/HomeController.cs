namespace Mp3MusicZone.Web.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.Contracts;
    using EfDataAccess;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using ViewModels.Songs;
    using Web.ViewModels;

    using static Common.Constants.WebConstants;

    public class HomeController : Controller
    {
        private ISongService songService;

        public HomeController(ISongService songService)
        {
            if (songService is null)
                throw new ArgumentNullException(nameof(songService));

            this.songService = songService;
        }

        public async Task<IActionResult> Index([FromServices]IConfiguration conf,
            [FromServices]MusicZoneDbContext context)
        {
            // sample admin logging
            //var logger = LogManager.GetLogger("AdminLogger");
            //logger.Trace("asd");
            IEnumerable<Song> lastSongs = await this.songService
                    .GetLastApprovedAsync(DefaultHomePageLastApprovedSongsCount);

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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
