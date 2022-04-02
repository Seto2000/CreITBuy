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
        public IActionResult Index()
        {
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["viewName"] = "Index";
            ViewData["controlerName"] = "Home";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AllProducts", "Product");
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