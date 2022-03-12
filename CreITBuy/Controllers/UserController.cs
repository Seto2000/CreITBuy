using Microsoft.AspNetCore.Mvc;

namespace CreITBuy.Controllers
{
    public class UserController: Controller

    {
        public IActionResult Login()
        {
            ViewData["viewName"] = "Login";
            ViewData["controlerName"] = "User";
            ViewData["isAuthorized"] = false;
            return View();
        }
        public IActionResult Register()
        {
            ViewData["viewName"] = "Register";
            ViewData["controlerName"] = "User";
            ViewData["isAuthorized"] = false;
            return View();
        }
    }
}
