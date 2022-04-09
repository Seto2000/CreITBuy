using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using CreITBuy.Infrastructures.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

#nullable disable
namespace CreITBuy.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IRepo repo;
        private readonly IValidationService validationService;

        public UserService(SignInManager<User> _signInManager,

            UserManager<User> _userManager,
            IRepo _repo,
            IValidationService _validationService
            )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            repo = _repo;
            validationService = _validationService;
        }

        
        public void SignOut()
        {
            signInManager.SignOutAsync();
        }
        public async Task<(User, SignInResult)> LoginAsync(LoginViewModel Input)
        {
            var user = await userManager.FindByEmailAsync(Input.Email);
            if(user != null)
            {

            
                if (!user.EmailConfirmed)
                {
                    return (user, SignInResult.Failed);
                }
                var result = await signInManager.PasswordSignInAsync(user,
                                       Input.Password, false, false);

                
                return (user, result);
            }
            return (null,null);
        }
        public async Task<(User,string,IdentityResult)> RegisterAsync(IFormFile fileObj, RegisterViewModel Input)
        {
            (bool isValid,string error)=validationService.ValidateModel(Input);
            if (isValid)
            {
                if (fileObj != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await fileObj.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        Input.Image = fileBytes;
                    }
                }
                else
                {
                    error += "; Image is required!";
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
                    if(code != null)
                    {
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    }
                    return (user, code, result);
                }
            }
            var identityErorrs = new List<IdentityError>();
            foreach (var item in error.Split(";", StringSplitOptions.RemoveEmptyEntries).ToArray())
            {
                identityErorrs.Add(new IdentityError() { Description=item.Trim() });
            }
           var result2 = IdentityResult.Failed(identityErorrs.ToArray());
                return (null,null,result2) ;
            

        }

        

        public (bool isValid, string errors) ValidateModel(RegisterViewModel input)
        {
            return validationService.ValidateModel(input);
        }
    }
}

