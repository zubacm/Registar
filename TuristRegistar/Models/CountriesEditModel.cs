﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class CountriesEditModel
    {
        public IEnumerable<Countries> AvailableCountries { get; set; }
        public String NewCountry { get; set; }

        public int EditCountryId { get; set; }
        public String EditCountryName { get; set; }

        public String SubmitButton { get; set; }
    }
}
