using CreITBuy.Core.Contracts;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
#nullable disable
namespace CreITBuy.Controllers
{
    public class CartController:Controller
    {
        private readonly ICartService cartService;
        private readonly IUserService userService;
        public CartController(ICartService _cartService,
            IUserService _userService)
        {
            userService = _userService;
            cartService = _cartService;
        }
        [Authorize]
        public async Task<IActionResult> MyCart()
        {
            ViewData["viewName"] = "MyCart";
            ViewData["controlerName"] = "Cart";
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            User user = userService.FindUserByName(User.Identity.Name);
            ViewData["User"] = user;
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
        public async Task<IActionResult> Checkout(string cartId,PaymentViewModel model)
        {

            (bool isCheckedout, List<(byte[] file, string name)> files) = cartService.Checkout(cartId);
            
            if (isCheckedout)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                            var i = 1;
                        foreach (var file in files)
                        {
                            var entry = archive.CreateEntry(file.name + ".zip", CompressionLevel.Fastest);
                            using (var zipStream = entry.Open())
                            {
                               await zipStream.WriteAsync(file.file, 0, file.file.Length);
                            }
                            i++;
                        }
                    }

                    return  File(ms.ToArray(), "application/zip", "Products.zip");
                }
                
            }
            ViewData["Errors"] = new List<string>() { "Cannot Checkout! Somethig went wrong!" };
            return View("Error");

        }
    }
}
