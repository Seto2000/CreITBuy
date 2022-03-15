using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CreITBuy.Core.Contracts
{
    public interface IValidationService
    {
        (bool isValid, string error) ValidateModel(object model);
    }
}
