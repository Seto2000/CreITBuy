using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
# nullable disable
namespace CreITBuy.Infrastructure.Data.Models
{
    public class UserJobRequest
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string FromUserId { get; set; }
        [ForeignKey(nameof(FromUserId))]
        public User FromUser { get; set; }
        [Required]
        [StringLength(36)]
        public string JobRequestId { get; set; }
        [ForeignKey(nameof(JobRequestId))]
        public JobRequest JobRequest { get; set; }
    }
}
