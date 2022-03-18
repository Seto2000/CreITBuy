using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
#nullable disable
namespace CreITBuy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(IndexViewModel user)
        {
            if(TempData["user"] != null)
            {
                user = JsonConvert.DeserializeObject<IndexViewModel>((string)TempData["user"]);
                ViewData["user"] = user;
                ViewData["UserImage"] = user.Image;
                ViewData["Username"] = user.Username;
                ViewData["Job"] = user.Job;
            }
            if(ViewData["user"] == null)
            {
                ViewData["IsAuthenticated"] = false;
            }
            else
            {
                ViewData["IsAuthenticated"] = true;

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
    }
}