using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.Product;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public async Task<(bool,string)> Add(ProductViewModel model,IFormFile[] images,IFormFile productArchive,User user)
        {
            (bool isValid,string errors)=validationService.ValidateModel(model);
            if (images.Length > 5 || images.Length==0)
            {
                isValid=false;
                errors += "; You have to add at least 1 and only 5 images!";
            }
            if (productArchive.Length == 0)
            {
                isValid = false;
                errors += "; Please, upload your product archive in format zip or rar!";
            }
            if (isValid)
            {
                
                Product product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Author= user,
                    AuthorId= user.Id,
                    PostedOn = model.PostedOn,
                    Tags=model.Tags,
                    Categories=model.Categories,
                    Price= model.Price
                };
            var imageList = new List<ProductImage>();
                using (var ms = new MemoryStream())
                {
                    await productArchive.CopyToAsync(ms);
                    var archiveBytes = ms.ToArray();
                    product.ProductArchive = archiveBytes;
                }
                foreach (var image in images)
                {
                    
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    imageList.Add(new ProductImage() 
                    {
                        Image = new Image()
                        {
                            ImageData = fileBytes 
                        },
                        ProductId = product.Id,
                        Product=product 
                    });
                    }

                }
            product.ProductImages = imageList;
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
           return repo.All<Product>().Include(p=>p.Author).Include(pi=>pi.ProductImages).ThenInclude(pi=>pi.Image).ToList();
        }

        public Image FindImageById(string imageId)
        {
            return repo.All<Image>().FirstOrDefault(i=>i.Id == imageId);
        }

        public Product FindProductById(string productId)
        {
            return repo.All<Product>().Include(p=>p.Author).Include(p=>p.ProductImages).ThenInclude(pi=>pi.Image).Where(p=>p.Id == productId).FirstOrDefault();
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
