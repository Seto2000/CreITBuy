using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Windows.Input;

namespace CreITBuy.Controllers
{
    
    public class UserController: Controller
    {
        private readonly IEmailSender emailSender;
        private readonly UserManager<User> userManager;

        private readonly IUserService userService;
        private readonly IProductService productService;

        public UserController(
            UserManager<User> _userManager,
            IProductService _productService,
            IEmailSender _emailSender,
            IUserService _userService
            )
        {
            userManager = _userManager;
            productService = _productService;
            emailSender = _emailSender;
            userService = _userService;
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
        public async Task<IActionResult> Details(string authorId)
        {
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            User user = userService.FindUserByName(User.Identity.Name);
            ViewData["User"]=user;
            
            ViewData["Products"] = productService.All().OrderByDescending(p=>p.PostedOn).Take(4).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormFile Image, RegisterViewModel Input)
        {
           
            if (ModelState.IsValid)
            {
                (User user, string code,IdentityResult result) = await userService.RegisterAsync(Image, Input);

                if (result.Succeeded)
                {
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Welcome to CreITBuy platform. Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." +
                        $"We hope you will have a great expiriance by joining our family." +
                        $"Have a great day!");

                        return View("ConfirmRegister");
                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            var modelErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    modelErrors.Add(error.ErrorMessage.Replace("field ",String.Empty));
                }
            }
            
                ViewData["Errors"] = new List<string>(modelErrors) ;
            return View("../Shared/Error");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel Input)
        {
            if (ModelState.IsValid)
            {


                (User user, Microsoft.AspNetCore.Identity.SignInResult result) =
                    await userService.LoginAsync(Input);
                if(user == null)
                {
                    ViewData["Errors"] = new List<string>() { "Email or password is invalid!" };
                    return View("../Shared/Error");
                }
                if (!user.EmailConfirmed)
                {
                    return View("ConfirmRegister");
                }
                
                if (result.Succeeded)
                {

                        var identity = new ClaimsIdentity("Custom");
                    HttpContext.User = new ClaimsPrincipal(identity);


                    return RedirectToAction("AllProducts", "Product");
                }
                
            }

            // If we got this far, something failed, redisplay form
            return Redirect("/User/Login");
        }
        [Authorize]
        public IActionResult Logout()
        {
           
            userService.SignOut();
            return RedirectToAction("Index","Home");
        }
        public IActionResult ConfirmRegister()
        {
            ViewData["viewName"] = "Confirm Register";
            ViewData["controlerName"] = "User";
            ViewData["isAuthenticated"] = User.Identity.IsAuthenticated;
            return View();
        }
    }
}
