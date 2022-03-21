using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Core.Contracts
{
    public interface IUserService
    {
        public string GetUsername(string userId);
        Task<(User, SignInResult, IndexViewModel)> LoginAsync(LoginViewModel Input);
        public Task<(User, string,IdentityResult)> RegisterAsync(IFormFile fileObj, RegisterViewModel Input);
        void SignOut();
    }
}
