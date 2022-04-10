using CreITBuy.Core.Contracts;
using CreITBuy.Core.Services;
using CreITBuy.Core.ViewModels.Product;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Test
{
    public class CartServiceTests
    {
        private ServiceProvider serviceProvider;
        private ICartService cartService;
        private InMemoryDbContext dbContext;
        private IRepo repo;
        private IProductService productService;
        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
               .AddSingleton(sp => dbContext.CreateContext())
               .AddSingleton<IRepo, Repo>()
               .AddSingleton<ICartService, CartService>()
               .AddSingleton<IProductService, ProductService>()
               .BuildServiceProvider();

            var repo = serviceProvider.GetService<IRepo>();

            this.repo = repo;
            productService = new ProductService(new ValidationService(), repo);
            cartService = new CartService(repo);
        }
        [Test]
        public async Task AddProductTest()
        {
            await AddProduct();
            string productId = repo.All<Product>().FirstOrDefault().Id;
            string username = "Berbatov2000";
            bool isAdded=cartService.AddProduct(productId, username);

            Assert.IsTrue(repo.All<Cart>().FirstOrDefault().Items.Count == 1);
            Assert.IsTrue(isAdded);

            bool isAdded2 = cartService.AddProduct("aadgsdgsg", "sdgsdgsgd");

            Assert.IsFalse(isAdded2);
        }

        [Test]
        public async Task RemoveItem()
        {
            await AddProduct();
            string productId = repo.All<Product>().FirstOrDefault().Id;
            string username = "Berbatov2000";
            bool isAdded = cartService.AddProduct(productId, username);

            Assert.IsTrue(repo.All<Cart>().FirstOrDefault().Items.Count == 1);

            string itemId = repo.All<Cart>().FirstOrDefault(x=>x.User.UserName == username).Items.FirstOrDefault().Id;
            cartService.RemoveItem(itemId, username);

            Assert.IsTrue(repo.All<Cart>().FirstOrDefault().Items.Count == 0);

            await AddProduct();
            bool isAdded2 = cartService.AddProduct(productId, username);

            Assert.IsTrue(repo.All<Cart>().FirstOrDefault().Items.Count == 1);

            cartService.RemoveItem("afaf", username);
            Assert.IsTrue(repo.All<Cart>().FirstOrDefault().Items.Count == 1);

        }

        [Test]
        public async Task GetCartByUsernameTest()
        {
            await AddProduct();
            string productId = repo.All<Product>().FirstOrDefault().Id;
            string username = "Berbatov2000";
            bool isAdded = cartService.AddProduct(productId, username);

            Cart cart = cartService.GetCartByUsername(username);

            Assert.IsNotNull(cart);
            Assert.IsNotNull(cart.Items);
            Assert.IsNotNull(cart.Items.FirstOrDefault().Product.ProductImages.FirstOrDefault().Image);
            Assert.IsNotNull(cart.Items.FirstOrDefault().Product.Author);

            Cart cart2 = cartService.GetCartByUsername("babbaab");
            Assert.IsNull(cart2);
        }

        [Test]
        public async Task CheckoutTest()
        {
            await AddProduct();
            string productId = repo.All<Product>().FirstOrDefault().Id;
            string username = "Berbatov2000";
            bool isAdded = cartService.AddProduct(productId, username);

            Assert.IsTrue(repo.All<Cart>().FirstOrDefault().Items.Count == 1);

            string cartId = repo.All<Cart>().FirstOrDefault().Id;

            (bool isCheckedout, List<(byte[] file, string name)> files) = cartService.Checkout(cartId);
            Assert.IsTrue(isCheckedout);
            Assert.IsNotNull(files);

            (bool isCheckedout2, List<(byte[] file, string name)> files2) = cartService.Checkout("asfafs");
            Assert.IsFalse(isCheckedout2);
            Assert.IsNull(files2);
        }

        private async Task<(bool isValid, string errors, User user)> AddProduct()
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
                Cart = new Cart()
            };

            (bool isValid, string errors) = await productService.Add(model, images, productArchive, user);

            return (isValid, errors, user);
        }
    }
}
