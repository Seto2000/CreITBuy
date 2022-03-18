using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.User;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using CreITBuy.Infrastructures.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace CreITBuy.Core.Services
{
    public class UserService : IUserService
    {

        private readonly ApplicationDbContext repo;

        private readonly IValidationService validationService;

        public UserService(
            ApplicationDbContext _repo,
            IValidationService _validationService
            )
        {
            repo = _repo;
            validationService = _validationService;
        }

        public string GetUsername(string userId)
        {
            return repo.Users
                .FirstOrDefault(u => u.Id == userId)?.Username;
        }

        public User Login(LoginViewModel model)
        {
            User user = repo.Users
                .SingleOrDefault(u=>u.Email == model.Email 
                && u.Password == CalculateHash(model.Password));
            return user;
        }

        public async Task<(bool registered, string error)> RegisterAsync(RegisterViewModel model,IFormFile fileObj)
        {
            bool registered = false;
            string error = string.Empty;

            var (isValid, validationError) = validationService.ValidateModel(model);
            

            if (!isValid)
            {
                return (isValid, validationError);
            }
          
            if (repo.Users.Any(u => u.Email == model.Email || u.Username == model.Username))
            {
                validationError = "Unexpected error!";

                return (false, validationError);
            }

            Cart cart = new Cart();
            User user = new User()
            {
                
                Username = model.Username,
                Email = model.Email,
                Password = CalculateHash(model.Password),
                Job = (Jobs)Enum.Parse(typeof(Jobs), model.Job),
                Cart = cart,
                CartId = cart.Id
            };
            if (fileObj.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await fileObj.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    user.Image = fileBytes;
                }
            }
            try
            {
                await repo.AddAsync(user);
                await repo.SaveChangesAsync();
                registered = true;
            }
            catch (Exception)
            {
                error = "Could not save user in DB";
            }

            return (registered, error);
        }

        private string CalculateHash(string password)
        {
            byte[] passworArray = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(passworArray));
            }
        }
    }
}

