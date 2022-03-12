using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CreITBuy.Infrastructure.Data.Models
{
    public class JobRequest
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(36)]
        public string FromUserId { get; set; }
        [Required]
        [ForeignKey(nameof(FromUserId))]
        public User FromUser { get; set; }
        [Required]
        [StringLength(36)]
        public string ToUserId { get; set; }
        [Required]
        [ForeignKey(nameof(ToUserId))]
        public User ToUser { get; set; }
        [Required]
        [StringLength(50)]
        public string Theme { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}