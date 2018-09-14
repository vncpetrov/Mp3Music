namespace Mp3MusicZone.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;

    [Authorize]
    public class SongsController : Controller
    {
        public IActionResult Upload()
        {
            return this.View();
        } 
    }
}
