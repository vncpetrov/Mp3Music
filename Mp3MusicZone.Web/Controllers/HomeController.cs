namespace Mp3MusicZone.Web.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Mp3MusicZone.EfDataAccess;
    using NLog;
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using Web.ViewModels;

    public class HomeController : Controller
    {
        public IActionResult Index([FromServices]IConfiguration conf, [FromServices]MusicZoneDbContext context)
        {
            // sample admin logging
            //var logger = LogManager.GetLogger("AdminLogger");
            //logger.Trace("asd");

            return View();
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
