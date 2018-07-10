namespace Mp3MusicZone.Web.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using NLog;
    using System;
    using System.Diagnostics;
    using Web.ViewModels;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // sample admin logging
            var logger = LogManager.GetLogger("AdminLogger");
            logger.Trace("asd");

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            throw new ArgumentException("Testing NLog db");
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
