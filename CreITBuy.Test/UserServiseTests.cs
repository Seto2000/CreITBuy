using CreITBuy.Core.Contracts;
using CreITBuy.Core.Services;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using CreITBuy.Infrastructures.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CreITBuy.Test
{
    public class UserServiceTests
    {
        private  IUserService userService;
        private UserManager<User> userManager;
        private List<User> users;
        private Repo repo;

        [SetUp]
        public void Setup()
        {
            List<User> _users = new List<User>();
            users = _users;

            repo =  new Repo(new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()));
            this.userService = userService;

        }
        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls,bool isTrue) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.Options.Password.RequireDigit = true;
            mgr.Object.Options.Password.RequireLowercase = true;
            mgr.Object.Options.Password.RequireUppercase = true;
            mgr.Object.Options.Password.RequiredLength = 6;
            mgr.Object.Options.Password.RequireNonAlphanumeric = false;

            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            if (isTrue)
            {
                mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));

            }
            else
            {
                mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            }
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(ls.FirstOrDefault()).Callback<string>(x=>ls.FirstOrDefault());
            return mgr;


        }

        [Test]
        public async Task LoginTest()
        {
            users.Add(new User()
            {
                UserName = "Berbatov2000",
                LiveIn = "Bulgaria, Sofia",
                PasswordHash = "AQAAAAEAACcQAAAAEI94JA2QBE2Eb0avL0VafpCfd7n0eubhaNSmBf4Y5MoAetMwMoVot8Pmj9aHM6u1/g==",
                Email = "setolan@abv.bg",
                NormalizedEmail = "SETOLAN@ABV.BG",
                Job = Jobs.Developer,
                EmailConfirmed = true,
                LockoutEnabled = false,
            }); 
            UserManager<User> userManager1 = MockUserManager(users, true).Object;
            IUserService userService = new UserService(new SignInManager<User>(userManager1 ,new HttpContextAccessor(), new Mock<IUserClaimsPrincipalFactory<User>>().Object,null, new Mock<ILogger<SignInManager<User>>>().Object,null,null), userManager1, repo, new ValidationService());
            (User user, SignInResult result) = await userService.LoginAsync(new LoginViewModel() { Email = "setolan@abv.bg", Password = "Seto" });
            Assert.IsFalse(result.Succeeded);
        }
       
        [Test]
        public async Task RegisterTest()
        {
            UserManager<User>  userManager1 = MockUserManager(users,true).Object;
            IUserService userService = new UserService(null, userManager1, repo, new ValidationService());

            var model = new RegisterViewModel()
            {
                Username = "Seto2000",
                Password = "Berbatov2000",
                ConfirmPassword = "Berbatov2000",
                LiveIn = "Bulgaria, Sofia",
                Email = "setolan@abv.bg",
                Job = "Developer",
            };
            var file = @"D:\download.jpg";
            using var stream = new MemoryStream(File.ReadAllBytes(file).ToArray());
            var formFile = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());
            (User user, string code, IdentityResult result) = await userService.RegisterAsync(formFile, model);
            Assert.IsTrue(result.Succeeded);
            Assert.IsNotNull(user);
            Assert.AreEqual(1,users.Count);
            UserManager<User> userManager2 = MockUserManager(users,false).Object;
            IUserService userService2 = new UserService(null, userManager2, repo, new ValidationService());

            var model2 = new RegisterViewModel()
            {
                Username = "setoto",
                Email = "setolan@abv.bg",
                Password = "erbatov",
                ConfirmPassword = "erbatov",
                LiveIn = "Bulgaria, Sofia",
                Job="Developer"
                
            };
            using var stream2 = new MemoryStream(File.ReadAllBytes(file).ToArray());
            var formFile2 = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());
            (User user2, string code2, IdentityResult result2) = await userService2.RegisterAsync(formFile2, model2);
            Assert.IsFalse(result2.Succeeded);
            Assert.AreEqual(1, users.Count);
        }
        [Test]
        public void ValidationTest()
        {
            userManager = MockUserManager(users, true).Object;
            IUserService userService = new UserService(null, userManager, repo, new ValidationService());

            var model = new RegisterViewModel()
            {
                Username = "Seto2000",
                Password = "Berbatov2000",
                ConfirmPassword = "Berbatov2000",
                LiveIn = "Bulgaria, Sofia",
                Email = "setolan@abv.bg",
                Job = "Developer",
                Image = null
            };
            Assert.IsTrue(userService.ValidateModel(model).isValid);
            Assert.IsEmpty(userService.ValidateModel(model).errors);

            var model2 = new RegisterViewModel()
            {
                Username = "Set",
                Password = "berba",
                ConfirmPassword = "Berbatov2000",
                LiveIn = "Bulgaria",
                Email = null,
                Job = null,
                Image = null
            };
            Assert.IsFalse(userService.ValidateModel(model2).isValid);
            Assert.AreEqual(6,userService.ValidateModel(model2).errors.Split("; ").Length);
        }
    }
}