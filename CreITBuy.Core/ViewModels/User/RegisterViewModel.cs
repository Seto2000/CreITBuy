﻿using System.ComponentModel.DataAnnotations;
#nullable disable
namespace CreITBuy.Core.ViewModels.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        public byte[] Image { get; set; }
        [Required(ErrorMessage ="{0} is required!")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required!")]

        [EmailAddress(ErrorMessage = "Email must be valid email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is required!")]

        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} must be between {2} and {1} characters!")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "{1} and {0} must be equal!")]
        public string ConfirmPassword { get; set; }
    }
}