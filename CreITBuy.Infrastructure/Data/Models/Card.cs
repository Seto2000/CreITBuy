using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CreITBuy.Infrastructure.Data.Models
{
    public class Card
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(100)]
        public string CardholderName { get; set; }
        [Required]
        [StringLength(36)]
        public string UserId { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [Required]
        [StringLength(64)]
        public string CardNumber { get; set; }
        [Required]
        [StringLength(64)]
        public string Cvc { get; set; }
        [Required]
        public DateOnly ValidThru { get; set; }

    }
}