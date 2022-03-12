using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CreITBuy.Infrastructure.Data.Models
{
    public class Cart
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(36)]
        public string UserId { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>(); 
    }
}