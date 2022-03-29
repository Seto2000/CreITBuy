using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace CreITBuy.Infrastructure.Data.Models
{
    public class ProductImage
    {
        [Required]
        [StringLength(36)]
        public string ProductId { get; set; }
        [Required]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        [Required]
        [StringLength(36)]

        public string ImageId { get; set; }
        [Required]
        [ForeignKey(nameof(ImageId))]
        public Image Image { get; set; }
    }
}
