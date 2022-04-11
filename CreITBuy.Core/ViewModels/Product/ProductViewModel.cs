using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace CreITBuy.Core.ViewModels.Product
{
    public class ProductViewModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100,MinimumLength =3, ErrorMessage = "{0} must be between {2} and {1} characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(3000, ErrorMessage ="{0} must be less then {1} characters!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be less then {1} characters!")]

        public string Categories { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be less then {1} characters!")]

        public string Tags { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [RegularExpression(@"(\d+\.\d{2})|(\d+)", ErrorMessage ="{0} must be in format \"0.00\" or \"0\"")]
        [Range(0.00, 100000.00, ErrorMessage ="{0} must be between {1} and {2}!")]
        public decimal Price { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
    }
}
