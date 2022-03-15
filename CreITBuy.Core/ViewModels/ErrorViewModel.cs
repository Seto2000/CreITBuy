using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Core.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string message)
        {
            ErrorMessage = message;
        }
        public string ErrorMessage { get; init; }
    }
}
