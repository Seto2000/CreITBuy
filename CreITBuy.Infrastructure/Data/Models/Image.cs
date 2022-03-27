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
        [Required]
        [StringLength(36)]
        public string Productid { get; set; }
        [Required]
        [ForeignKey(nameof(Productid))]
        public Product Product { get; set; }
        [Required]
        public byte[] ImageData { get; set; }
    }
}