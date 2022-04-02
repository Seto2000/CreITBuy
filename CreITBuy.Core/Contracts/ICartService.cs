using CreITBuy.Infrastructure.Data.Models;
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
        bool Checkout(string cartId);
    }
}
