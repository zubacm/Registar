using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class CurrencyEditModel
    {
        public String NewCurrency { get; set; }
        public IEnumerable<SelectListItem> AllCurrenciesFromAPI { get; set; }
        
        public IEnumerable<Currencies> AvailableCurrencies { get; set; }

    }
}
