using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.Product;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
#nullable disable
namespace CreITBuy.Controllers
{
    public class ProductController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;
        public ProductController(UserManager<User> _userManager, IProductService _productService)
        {
            userManager = _userManager;
            productService = _productService;
        }
        [Authorize]
        public async Task<IActionResult> AddProduct()
        {
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["viewName"] = "Add";
            ViewData["controlerName"] = "Product";
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(IFormFile fileObj,ProductViewModel Input)
        {
            (bool isAdded, string errors) = await productService
                .Add(Input, fileObj, await userManager
                .FindByNameAsync(User.Identity.Name));
            if (isAdded)
            {
                return Redirect("/");
            }
            else
            {
                return View("Error",errors);
            }
        }

    }
}
