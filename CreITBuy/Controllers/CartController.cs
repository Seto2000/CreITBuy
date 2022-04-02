using CreITBuy.Core.Contracts;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
#nullable disable
namespace CreITBuy.Controllers
{
    public class CartController:Controller
    {
        private readonly ICartService cartService;
        private readonly UserManager<User> userManager;
        public CartController(ICartService _cartService,UserManager<User> _userManager)
        {
            cartService = _cartService;
            userManager = _userManager;
        }
        [Authorize]
        public async Task<IActionResult> MyCart()
        {
            ViewData["viewName"] = "MyCart";
            ViewData["controlerName"] = "Cart";
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            User user = await userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserImage"] = user.Image;
            ViewData["Username"] = user.UserName;
            ViewData["Job"] = user.Job;
            Cart cart = cartService.GetCartByUsername(User.Identity.Name);
            ViewData["Cart"]=cart;
            return View();
        }
        public IActionResult Add(string productId)
        {
            bool isAdded = cartService.AddProduct(productId,User.Identity.Name);
            if (isAdded)
            {
                return Redirect("/");
            }
            ViewData["Errors"] = new List<string>() { "Cannot add product to Cart!" };
            return View("Error");
        }
        public IActionResult Remove(string itemId)
        {
            bool isRemoved = cartService.RemoveItem(itemId, User.Identity.Name);
            if (isRemoved)
            {
                return Redirect("/Cart/MyCart");
            }
            ViewData["Errors"]= new List<string>() { "Cannot add product to Cart!" };
            return View("Error");
        }
        public IActionResult Checkout(string cartId,PaymentViewModel model)
        {
            
            bool isCheckedout = cartService.Checkout(cartId);
            if (isCheckedout)
            {
                return Redirect("/");
            }
            ViewData["Errors"] = new List<string>() { "Cannot Checkout! Somethig went wrong!" };
            return View("Error");

        }
    }
}
