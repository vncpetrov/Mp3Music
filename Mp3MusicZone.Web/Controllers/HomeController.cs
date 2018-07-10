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
    using Web.ViewModels;

    public class HomeController : Controller
    {
        public IActionResult Index([FromServices]IConfiguration conf)
        {
            string connStr = conf.GetConnectionString("MusicZoneConnectionString");

            // sample admin logging
            var logger = LogManager.GetLogger("AdminLogger");
            logger.Trace("asd");

            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT COUNT(*) FROM ErrorLogs", connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var cnt1 = reader[0];
            reader.Close();

            SqlCommand command2 = new SqlCommand(@"SELECT COUNT(*) FROM AdminLogs", connection);
            var reader2 = command2.ExecuteReader();
            reader2.Read();
            var cnt2 = reader2[0];

            return View(new object[] { connStr, cnt1, cnt2 });
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
