using CreITBuy.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace CreITBuy.Infrastructure.Data.Models
{
    public class User 
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [StringLength(20)]
        [Required]
        public string Username { get; set; }
        [EmailAddress]
        [StringLength(300)]
        [Required]
        public string Email { get; set; }
        [StringLength(64)]
        [Required]
        public string Password { get; set; }
        [StringLength(15)]
        [Required]
        public Jobs Job { get; set; }

        [Required]
        [Column(TypeName ="image")]
        public byte[] Image { get; set; }
        [Required]
        [StringLength(36)]
        public string CartId { get; set; }
        [Required]
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }
        public IList<Notification> Notifications { get; set; } = new List<Notification>();
        public IList<Card> Cards { get; set; } = new List<Card>();
        public IList<UserJobRequest> UserJobRequests { get; set; } = new List<UserJobRequest>();
        public IList<Product> Products { get; set; } = new List<Product>();

    }
}
