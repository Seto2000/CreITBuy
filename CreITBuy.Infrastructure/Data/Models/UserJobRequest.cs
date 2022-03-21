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
        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [Required]
        [StringLength(36)]
        public string JobRequestId { get; set; }
        [ForeignKey(nameof(JobRequestId))]
        public JobRequest JobRequest { get; set; }
    }
}
