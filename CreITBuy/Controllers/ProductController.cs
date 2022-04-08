using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.Product;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            ViewData["User"] = user;
            if (user.Job == Jobs.Client)
            {
                ViewData["Errors"]= new List<string>() { "You do not have permission to access this page!" };
                return View("Error");
            }
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483648)]
        [RequestSizeLimit(2147483648)]
        public async Task<IActionResult> AddProduct(IFormFile[] fileObj,IFormFile productArchive,ProductViewModel Input)
        {
            if(fileObj == null || productArchive == null || Input == null)
            {
                ViewData["Errors"] = new List<string>() { "You are trying to upload too large files or somthing went wrong!" };
                  
                return View("Error");
            }
            (bool isAdded, string errors) = await productService
                .Add(Input, fileObj, productArchive, await userManager
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
        public async Task<IActionResult> AllProducts(string tags)
        {
            if (string.IsNullOrEmpty(tags))
            {
                ViewData["viewName"] = "All";
                ViewData["controlerName"] = "Products";
                ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
                ViewData["isSearching"] = false;
                ViewData["Products"] = new List<Product>(productService.All());
                User user = await userManager.FindByNameAsync(User.Identity.Name);
                ViewData["User"] = user;
            }
            else
            {
                List<Product> products = new List<Product>();
                List<string> tagsArr = new List<string>(tags.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries));
                products = productService
                    .All()
                    .Where(p => p.Tags
                    .Split(", ")
                    .Any(tag => tagsArr.Contains(tag.ToLower())) ||
                    tagsArr.Contains(p.Name.ToLower()) || 
                    tagsArr.Contains(p.Author.UserName.ToLower()) ||
                    p.Categories.Split(", ").Any(tag => tagsArr.Contains(tag.ToLower())))
                    .ToList();

                ViewData["viewName"] = "Search";
                ViewData["controlerName"] = "Products";
                ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
                ViewData["isSearching"] = true;
                ViewData["Products"] = products;
                User user = await userManager.FindByNameAsync(User.Identity.Name);
                ViewData["User"] = user;
            }
            
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Details(string productId,string imageId)
        {
            ViewData["viewName"] = "Details";
            ViewData["controlerName"] = "Products";
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            User user = await userManager.FindByNameAsync(User.Identity.Name);
            ViewData["User"] = user;
            Product product = productService.FindProductById(productId);
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
