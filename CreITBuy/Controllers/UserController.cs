using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Security.Claims;

namespace CreITBuy.Controllers
{
    
    public class UserController: Controller
    {

        private readonly IWebHostEnvironment iweb;

        private readonly IUserService userService;
        public UserController(
            IUserService _userService,
            IWebHostEnvironment _iweb)
        {
            userService = _userService;
            iweb = _iweb;
        }
        public IActionResult Login()
        {
            ViewData["viewName"] = "Login";
            ViewData["controlerName"] = "User";
            ViewData["isAuthenticated"] = false;
            return View();
        }
        public IActionResult Register()
        {

            ViewData["viewName"] = "Register";
            ViewData["controlerName"] = "User";
            ViewData["isAuthenticated"] = false;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormFile fileObj,RegisterViewModel model)
        {
            (bool registered, string error) = await userService.RegisterAsync(model,fileObj);
            if (!registered)
            {
                return View("../Shared/Error", error);
            }
            return Redirect("/User/Login");
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = userService.Login(model);
            if (user == null)
            {
                return View("../Shared/Error", "Email or Password is incorrect!");
            }
            else
            {
                var identity = new ClaimsIdentity("Custom");
                HttpContext.User = new ClaimsPrincipal(identity);

            }
            SignIn(HttpContext.User);
            var indexModel = new IndexViewModel()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Job = user.Job.ToString(),
                Image = user.Image,
                Password = user.Password,
            };
            TempData["user"] = JsonConvert.SerializeObject(indexModel);
            return Redirect("/");
        }

        public IActionResult Logout()
        {
            SignOut();
            return Redirect("/");
        }
    }
}
