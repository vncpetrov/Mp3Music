namespace Mp3MusicZone.Web.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.Contracts;
    using DomainServices.QueryServices;
    using DomainServices.QueryServices.Songs.GetLastApproved;
    using EfDataAccess;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Mp3MusicZone.EfDataAccess.Models;
    using Mp3MusicZone.Web.ViewModels.Shared;
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

        public HomeController(
            IQueryService<GetLastApprovedSongs, IEnumerable<Song>> getSongs)
        {
            if (getSongs is null)
                throw new ArgumentNullException(nameof(getSongs));

            this.getSongs = getSongs;
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

        public IActionResult About([FromServices]MusicZoneDbContext context)
        {
            List<string> list = context.Permissions.Select(p=>$"{p.Name} - {p.Id}").ToList();
            return View(list);
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
