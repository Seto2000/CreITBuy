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
           
            ViewData["UserImage"] = user.Image;
            ViewData["Username"] = user.UserName;
            ViewData["Job"] = user.Job;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(IFormFile[] fileObj,ProductViewModel Input)
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
                ViewData["Errors"] = new List<string>
                    (errors.Split(";",StringSplitOptions.RemoveEmptyEntries)
                    .ToList());
                return View("Error",errors);
            }
        }
        [Authorize]
        public async Task<IActionResult> AllProducts()
        {
            ViewData["viewName"] = "All";
            ViewData["controlerName"] = "Products";
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;

            ViewData["Products"] = new List<Product>(productService.All());
            User user = await userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserImage"] = user.Image;
            ViewData["Username"] = user.UserName;
            ViewData["Job"] = user.Job;
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Details(string productId,string imageId)
        {
            ViewData["viewName"] = "Details";
            ViewData["controlerName"] = "Products";
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            User user = await userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserImage"] = user.Image;
            ViewData["Username"] = user.UserName;
            ViewData["Job"] = user.Job; Product product = productService.FindProductById(productId);
            ViewData["Product"] = product;
            if(imageId != null)
            {
                ViewData["Image"] = productService.FindImageById(imageId);
            }
            else
            {
                ViewData["Image"] = product.ProductImages.FirstOrDefault().Image;
            }
            return View();
        }
    }
}
