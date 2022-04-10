using CreITBuy.Core.Contracts;
using CreITBuy.Core.Services;
using CreITBuy.Core.ViewModels.Product;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using CreITBuy.Infrastructure.Data.Repositoryes;
using CreITBuy.Infrastructures.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Test
{
    public class ProductServiceTests
    {
        private ServiceProvider serviceProvider;
        private ValidationService validationService;
        private List<Product> products;
        private IProductService productService;
        private InMemoryDbContext dbContext;
        private IRepo repo;
        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
               .AddSingleton(sp => dbContext.CreateContext())
               .AddSingleton<IRepo, Repo>()
               .AddSingleton<IProductService, ProductService>()
               .BuildServiceProvider();

            var repo = serviceProvider.GetService<IRepo>();

            this.repo = repo;
            validationService =new ValidationService ();

            productService = new ProductService(validationService,repo);
        }
       
        
        [Test]
        public async Task AddTest()
        {
           

            (bool isValid, string errors,User user) = await AddProduct();

            Assert.IsTrue(isValid);
            Assert.AreEqual(null,errors);
            Assert.IsTrue(repo.All<Product>().Count()==1);

            ProductViewModel model2 = new ProductViewModel()
            {
                Name = "G",
                Price = 111111111111
            };

            (bool isValid2, string errors2) = await productService.Add(model2, new FormFile[0], new FormFile(null,0,0,null,null), user);

            Assert.IsFalse(isValid2);
            Assert.AreEqual(7, errors2.Split("; ").Length);
            Assert.IsTrue(repo.All<Product>().Count() == 1);

        }

        [Test]
        public async Task AllTest()
        {
            
             await AddProduct();

            List<Product> products = (List<Product>)productService.All();

            Assert.IsTrue(products.Count() == 1);
            Assert.IsNotNull(products[0].Author);
            Assert.IsNotNull(products[0].ProductImages.FirstOrDefault().Image);
        }

        [Test]
        public async Task FindImageByIdTest()
        {
            await AddProduct();
            string imageId = repo.All<Image>().FirstOrDefault().Id;
            Image image = productService.FindImageById(imageId);
            Assert.IsNotNull(image);

            Image image2 = productService.FindImageById("asfoasfoif");
            Assert.IsNull(image2);

        }

        [Test]
        public async Task FindProductByIdTest()
        {
            await AddProduct();

            string productId = repo.All<Product>().FirstOrDefault().Id;
            Product product = productService.FindProductById(productId);

            Assert.IsNotNull(product);
            Assert.IsNotNull(product.Author);
            Assert.IsNotNull(product.ProductImages.FirstOrDefault().Image);

            Product product2 = productService.FindProductById("anfonfgn");

            Assert.IsNull(product2);
        }
        [Test]
        public async Task RemoveTest()
        {
            await AddProduct();

            Assert.IsTrue(repo.All<Product>().Count()==1);
            
            string productId = repo.All<Product>().FirstOrDefault().Id;
            (bool isRemoved, string error) = productService.Remove(productId);

            Assert.IsTrue(isRemoved);
            Assert.IsTrue(error==null);
            Assert.IsTrue(repo.All<Product>().Count() == 0);

            await AddProduct();

            (bool isRemoved2, string error2) = productService.Remove("asdgsdgag");

            Assert.IsFalse(isRemoved2);
            Assert.IsTrue(error2 != null);
            Assert.IsTrue(repo.All<Product>().Count() == 1);
        }

        private async Task<(bool isValid, string errors,User user)> AddProduct()
        {
            ProductViewModel model = new ProductViewModel()
            {
                Name = "Game",
                Description = "Lorum ipsum ne pilom korim bahim jasdj.",
                PostedOn = DateTime.Now,
                Tags = "bala, mssql, jsdjfj, kjdj",
                Categories = "asfdafs, asfasf, asf, adsf",
                Price = 100
            };

            var file = @"D:\download.jpg";
            using var stream = new MemoryStream(File.ReadAllBytes(file).ToArray());
            var formFile = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());

            FormFile[] images = new List<FormFile>()
            {
                formFile,
                formFile,
                formFile
            }.ToArray();

            FormFile productArchive = formFile;

            User user = new User()
            {
                UserName = "Berbatov2000",
                LiveIn = "Bulgaria, Sofia",
                PasswordHash = "AQAAAAEAACcQAAAAEI94JA2QBE2Eb0avL0VafpCfd7n0eubhaNSmBf4Y5MoAetMwMoVot8Pmj9aHM6u1/g==",
                Email = "setolan@abv.bg",
                NormalizedEmail = "SETOLAN@ABV.BG",
                Job = Jobs.Developer,
                EmailConfirmed = true,
                LockoutEnabled = false,
            };

            (bool isValid, string errors) = await productService.Add(model, images, productArchive, user);

            return (isValid,errors,user);
        }
    }
}
