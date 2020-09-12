using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class Cities
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public int CountriesId { get; set; }
        public Countries Country { get; set; }

        public Double Lat { get; set; }
        public Double Lng { get; set; }
    }
}
