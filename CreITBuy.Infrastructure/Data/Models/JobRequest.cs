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

        public IList<UserJobRequest> UserJobRequests { get; set; }
        [Required]
        [StringLength(50)]
        public string Theme { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}