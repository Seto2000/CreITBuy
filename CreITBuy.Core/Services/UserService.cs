using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using CreITBuy.Infrastructures.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

#nullable disable
namespace CreITBuy.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailSender emailSender;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext repo;

        private readonly IValidationService validationService;

        public UserService(SignInManager<User> _signInManager,

            IEmailSender _emailSender,
            UserManager<User> _userManager,
            ApplicationDbContext _repo,
            IValidationService _validationService
            )
        {
            emailSender = _emailSender;
            userManager = _userManager;
            signInManager = _signInManager;
            repo = _repo;
            validationService = _validationService;
        }

        public string GetUsername(string userId)
        {
            return repo.Users
                .FirstOrDefault(u => u.Id == userId)?.UserName;
        }
        public void SignOut()
        {
            signInManager.SignOutAsync();
        }
        public async Task<(User, SignInResult)> LoginAsync(LoginViewModel Input)
        {
            var user = await userManager.FindByEmailAsync(Input.Email);
            var result = await signInManager.PasswordSignInAsync(user,
                                   Input.Password, false, false);

            if (user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                return (user, result);
            }
            return (null, result);
        }
        public async Task<(User,string,IdentityResult)> RegisterAsync(IFormFile fileObj, RegisterViewModel Input)
        {

            if (fileObj!=null)
            {
                using (var ms = new MemoryStream())
                {
                    await fileObj.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    Input.Image = fileBytes;
                }
            }
            var user = new User 
            { 
                UserName = Input.Username,
                Email = Input.Email,
                Image = Input.Image,
                Cart = new Cart(),
                Job = (Jobs)Enum.Parse(typeof(Jobs), Input.Job),
                LockoutEnabled = false
            };
            var result = await userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {

                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                return (user,code,result);
            }
            
                return(null,null,result);
            

        }

        private string CalculateHash(string password)
        {
            byte[] passworArray = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(passworArray));
            }
        }

        public (bool isValid, string errors) ValidateModel(RegisterViewModel input)
        {
            return validationService.ValidateModel(input);
        }
    }
}

