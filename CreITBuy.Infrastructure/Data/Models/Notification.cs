using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CreITBuy.Infrastructure.Data.Models
{
    public class Notification
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
        [Required]
        [StringLength(36)]
        public string UserId { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
