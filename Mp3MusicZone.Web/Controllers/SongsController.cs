namespace Mp3MusicZone.Web.Controllers
{
    using Domain.Contracts;
    using Domain.Models;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Mp3MusicZone.DataServices.Contracts;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ViewModels.Songs;

    using static Common.Constants.ModelConstants;

    [Authorize]
    public class SongsController : Controller
    {
        private readonly ISongService songService;

        public SongsController(ISongService songService)
        {
            if (songService is null)
                throw new ArgumentNullException(nameof(songService));

            this.songService = songService;
        }

        public IActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Upload(UploadSongViewModel model)
        {
            if (model.File is null)
            {
                return View(model)
                    .WithErrorMessage("Please, choose a file!");
            }

            if (!model.File.IsSong() || model.File.Length > SongMaxLength)
            {
                return View(model)
                    .WithErrorMessage($"Your submission should be an audio file and no more than {SongMaxLength} MBs in size!");
            }

            try
            {
                string songExtension = model.File
                    .ContentType
                    .Split("/")
                    .Last();

                await this.songService.UploadAsync(
                     model.Title,
                     songExtension,
                     model.Singer,
                     model.ReleasedYear,
                     this.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                     model.File.ToByteArray());
            }
            catch (Exception)
            {
                return View(model)
                    .WithErrorMessage("We're sorry, something went wrong. Please try again later.");
            }

            return View()
                .WithSuccessMessage("Song uploaded successfully.");
        }
    }
}
