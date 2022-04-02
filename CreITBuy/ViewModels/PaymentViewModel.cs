#nullable disable
using System.ComponentModel.DataAnnotations;

namespace CreITBuy.ViewModels
{
    public class PaymentViewModel
    {
        [Required(ErrorMessage ="{0} is required!")]
        public string CardType { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string CardholderName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string Cvc { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string ValidTill { get; set; }
        public bool IsSaving { get; set; }
    }
}
