using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.Product;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace CreITBuy.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IValidationService validationService;
        private readonly IRepo repo;
        public ProductService(IValidationService _validationService, IRepo _repo)
        {
            validationService = _validationService;
            repo = _repo;
        }
        public async Task<(bool,string)> Add(ProductViewModel model,IFormFile image,User user)
        {
            (bool isValid,string errors)=validationService.ValidateModel(model);
            if (isValid)
            {
                using (var ms = new MemoryStream())
                {
                    await image.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    model.Image = fileBytes;
                }
                Product product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Author= user,
                    AuthorId= user.Id,
                    PostedOn = model.PostedOn,
                    Price= model.Price,
                    Image= model.Image
                };
                try
                {
                    repo.Add<Product>(product);
                    repo.SaveChanges();
                }
                catch 
                {
                    return (false, "Cannot add product to Database!");
                }
                return (true,null);
            }
            return (false,errors);
        }

        public IList<Product> All()
        {
           return repo.All<Product>().ToList();
        }

        public (bool,string) Remove(string productId)
        {
            Product product=repo.All<Product>().SingleOrDefault(p=>p.Id==productId);
            if (product == null)
            {
                return (false,"Cannot find product from Database");
            }
            else
            {
                try
                {
                    repo.Remove<Product>(product);
                    repo.SaveChanges();
                }
                catch
                {
                    return (false, "Cannot remove product from Database!");
                }
                return (true, null);
            }
        }
    }
}
