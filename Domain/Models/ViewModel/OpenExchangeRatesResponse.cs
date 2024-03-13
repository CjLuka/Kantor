using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ViewModel
{
    public class OpenExchangeRatesResponse
    {
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
