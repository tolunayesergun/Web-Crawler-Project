using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebCrawlerProject.Helpers;
using WebCrawlerProject.Models;

namespace WebCrawlerProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Index(string[] Site)
        {
            var allSiteData = new List<UrlModel>();
            if (Site != null && Site.Length > 0)
            {
                foreach (var item in Site)
                {
                    allSiteData.Add(HtmlParser.GetPageInfoByUrl(item));
                }
            }

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public ActionResult viewPart1()
        {

            return PartialView("~/Views/PartialViews/partialPart1.cshtml");
        }
    }
}
