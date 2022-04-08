using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Core.Contracts
{
    public interface ICartService
    {
        Cart GetCartByUsername(string? name);
        bool AddProduct(string productId,string name);
        bool RemoveItem(string itemId, string name);
        (bool isCheckedout, List<(byte[] file, string name)> files) Checkout(string cartId);
    }
}
