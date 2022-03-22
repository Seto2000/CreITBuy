using CreITBuy.Core.ViewModels.Product;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Core.Contracts
{
    public interface IProductService
    {
        Task<(bool, string)> Add(ProductViewModel model, IFormFile image, User user);
        public (bool, string) Remove(string productId);
        IList<Product> All();
    }
}
