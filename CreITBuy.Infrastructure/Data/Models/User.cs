using CreITBuy.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace CreITBuy.Infrastructure.Data.Models
{
    public class User:IdentityUser
    {
        
        [StringLength(15)]
        [Required]
        public Jobs Job { get; set; }

        [Required]
        [Column(TypeName ="image")]
        public byte[] Image { get; set; }
        [Required]
        [StringLength(100)]
        public string  LiveIn { get; set; }
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
