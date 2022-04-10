using CreITBuy.Core.Services;
using CreITBuy.Core.ViewModels.Product;
using CreITBuy.Core.ViewModels.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Test
{
    public class ValidationServiceTests
    {
        private ValidationService validationService; 
        [SetUp]
        public void Setup()
        {
           validationService = new ValidationService();

        }
        [Test]
        public void ValidateRegiserTest()
        {
            var model = new RegisterViewModel()
            {
                Username = "Seto2000",
                Password = "Berbatov2000",
                ConfirmPassword = "Berbatov2000",
                LiveIn = "Bulgaria, Sofia",
                Email = "setolan@abv.bg",
                Job = "Developer",
            };
            (bool isValid, string errors) = validationService.ValidateModel(model);
            Assert.IsTrue(isValid);
            Assert.AreEqual("",errors);


            var model2 = new RegisterViewModel()
            {
                Username = "s",
                Password = "ad",
                ConfirmPassword = "afsaff",
                LiveIn = "Bfia",
                Email = null,
                Job = null,
            };
            (bool isValid2, string errors2) = validationService.ValidateModel(model2);
            Assert.IsFalse(isValid2);
            Assert.AreEqual(6, errors2.Split("; ").Length);
        }


        [Test]
        public void ValidateProductTest()
        {
            var model = new ProductViewModel()
            {
                Name = "Game",
                Description = "Lorum ipsum ne pilom korim bahim jasdj.",
                PostedOn = DateTime.Now,
                Tags = "bala, mssql, jsdjfj, kjdj",
                Categories = "asfdafs, asfasf, asf, adsf",
                Price = 100
            };
            (bool isValid, string errors)= validationService.ValidateModel(model);
            Assert.IsTrue(isValid);
            Assert.AreEqual("", errors);

            var model2 = new ProductViewModel()
            {
                Name = "G",
                Price = 111111111111
            };
            (bool isValid2, string errors2) = validationService.ValidateModel(model2);

            Assert.IsFalse(isValid2);
            Assert.AreEqual(5, errors2.Split("; ").Length);

        }
    }
}
