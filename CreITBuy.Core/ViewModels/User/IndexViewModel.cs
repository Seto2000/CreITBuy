using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace CreITBuy.Core.ViewModels.User
{
    public class IndexViewModel
    {
        public string Id { get; set; } 
      
        public string Username { get; set; }
     
        public string Email { get; set; }
     
        public string Password { get; set; }
        
        public string Job { get; set; }

        public byte[] Image { get; set; }
    }
}
