using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace CreITBuy.Infrastructure.Data.Models
{
    public class Image
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public IList<ProductImage> ProductImages { get; set; }
        [Required]
        [Column(TypeName = "image")]
        public byte[] ImageData { get; set; }
    }
}