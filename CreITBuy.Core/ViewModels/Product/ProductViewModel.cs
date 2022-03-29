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
        [StringLength(100, ErrorMessage = "{0} must be less then {1} characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(1000, ErrorMessage ="{0} must be less then {1} characters!")]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be less then {1} characters!")]

        public string Categories { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be less then {1} characters!")]

        public string Tags { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0.00, 100000.00, ErrorMessage ="{0} must be between {1} and {2}!")]
        public decimal Price { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
    }
}
