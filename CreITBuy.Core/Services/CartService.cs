using CreITBuy.Core.Contracts;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace CreITBuy.Core.Services
{
    public class CartService:ICartService
    {
        private readonly IRepo repo;
        public CartService(IRepo _repo)
        {
            repo = _repo;
        }

        public bool AddProduct(string productId,string name)
        {
            try
            {
                var product = repo.All<Product>().Include(p=>p.Author).Include(p=>p.ProductImages).FirstOrDefault(p=>p.Id==productId);
                var item = new Item() { Product=product, ProductId=productId};
                item.Product.Author = product.Author;
                repo.All<Cart>().FirstOrDefault(c => c.User.UserName == name).Items.Add(item);
                repo.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public (bool isCheckedout, List<(byte[] file,string name)> files) Checkout(string cartId)
        {
            try
            {
                var cart = repo.All<Cart>()
                    .Include(c=>c.Items).ThenInclude(i=>i.Product)
                    .FirstOrDefault(c=>c.Id==cartId);
                List<(byte[] file, string name)> files = new List<(byte[] file, string name)>();
                foreach(var items in cart.Items)
                {
                    files.Add((items.Product.ProductArchive,items.Product.Name));
                }
                cart.Items.Clear();
                repo.SaveChanges();
                return (true,files);
            }
            catch 
            {
                return (false,null); 
            }
        }

        public Cart GetCartByUsername(string name)
        {
            return repo.All<Cart>()
                .Include(c=>c.Items)
                .ThenInclude(i=>i.Product)
                .ThenInclude(p=>p.ProductImages)
                .ThenInclude(pi=>pi.Image)
                .Include(c=>c.Items)
                .ThenInclude(i=>i.Product)
                .ThenInclude(p=>p.Author)
                .FirstOrDefault(c=>c.User.UserName==name);
        }

        public bool RemoveItem(string itemId, string name)
        {
            try
            {
                var item = repo.All<Item>().FirstOrDefault(p=>p.Id==itemId);
                repo.All<Cart>().FirstOrDefault(c => c.User.UserName == name).Items.Remove(item);
                repo.SaveChanges();
                return true;
            }
            catch 
            {
                return false; 
            }
        }
    }
}
