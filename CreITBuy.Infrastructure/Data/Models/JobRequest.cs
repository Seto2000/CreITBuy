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

        public UserJobRequest FromUserJobRequest { get; set; }
        [Required]
        [StringLength(50)]
        public string Theme { get; set; }
        [Required]
        [StringLength(3000)]
        public string Description { get; set; }
        [Required]
        public string  ToUserId { get; set; }
        [Required]
        [ForeignKey(nameof(ToUserId))]
        public User ToUser { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}