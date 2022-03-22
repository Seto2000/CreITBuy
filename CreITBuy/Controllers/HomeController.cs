using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
#nullable disable
namespace CreITBuy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;
        public HomeController(ILogger<HomeController> _logger, UserManager<User> _userManager, IProductService _productService)
        {
            this.logger = _logger;
            userManager = _userManager;
            productService = _productService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Products"] = new List<Product>(productService.All());
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["viewName"] = "Index";
            ViewData["controlerName"] = "Home";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (TempData["user"] != null)
                {
                    var user = JsonConvert.DeserializeObject<IndexViewModel>((string)TempData["user"]);
                    ViewData["user"] = user;
                    ViewData["UserImage"] = user.Image;
                    ViewData["Username"] = user.Username;
                    ViewData["Job"] = user.Job;
                }
                else
                {
                    User user = await userManager.FindByNameAsync(User.Identity.Name);
                    var indexModel = new IndexViewModel()
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Job = user.Job.ToString(),
                        Image = user.Image,

                    };
                    ViewData["UserImage"] = user.Image;
                    ViewData["Username"] = user.UserName;
                    ViewData["Job"] = user.Job;
                }
            }
          
            return View();
        }

        public IActionResult Privacy()
        {
            if (ViewData["user"] == null)
            {
                ViewData["IsAuthenticated"] = false;
            }
            else
            {
                ViewData["IsAuthenticated"] = true;

            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}