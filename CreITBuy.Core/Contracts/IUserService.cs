using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
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
        public User Login(LoginViewModel model);
        public Task<(bool registered, string error)> RegisterAsync(RegisterViewModel model, IFormFile fileObj);
    }
}
