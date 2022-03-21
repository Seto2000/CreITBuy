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
        
        private readonly IUserService userService;

        public UserController(
            IEmailSender _emailSender,
            IUserService _userService
            )
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormFile fileObj, RegisterViewModel Input)
        {
            if (ModelState.IsValid)
            {
                (User user, string code,IdentityResult result) = await userService.RegisterAsync(fileObj, Input);

                if (result.Succeeded)
                {
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        return RedirectToPage("RegisterConfirmation",
                                              new { email = Input.Email });
                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Redirect("/User/Register");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel Input)
        {
            if (ModelState.IsValid)
            {


                (User user, Microsoft.AspNetCore.Identity.SignInResult result, IndexViewModel indexModel) =
                    await userService.LoginAsync(Input);

                if (user!=null)
                {

                        var identity = new ClaimsIdentity("Custom");
                    HttpContext.User = new ClaimsPrincipal(identity);


                    TempData["user"] = JsonConvert.SerializeObject(indexModel);
                    return RedirectToAction("Index", "Home");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new
                    {
                        ReturnUrl = "/",
                    });
                }
                if (result.IsLockedOut)
                {
                    return RedirectToPage("../Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Redirect("User/Login");
                }
            }

            // If we got this far, something failed, redisplay form
            return Redirect("User/Login");
        }
        [Authorize]
        public IActionResult Logout()
        {
           
            userService.SignOut();
            return RedirectToAction("Index","Home");
        }
    }
}
