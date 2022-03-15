using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable
namespace CreITBuy.Core.Services
{
    public class ValidationService : IValidationService
    {
        public (bool isValid, string error) ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var errorResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, errorResult, true);
            string error = String.Join("; ", errorResult.Select(u => new ErrorViewModel(u.ErrorMessage).ErrorMessage).ToList());


                return (isValid,error );
            
        }
    }
}
