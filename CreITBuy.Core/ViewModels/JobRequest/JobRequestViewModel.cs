using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Core.ViewModels.JobRequest
{
    public class JobRequestViewModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50,MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters!")]
        public string Theme { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters!")]
        public string Description { get; set; }
    }
}
