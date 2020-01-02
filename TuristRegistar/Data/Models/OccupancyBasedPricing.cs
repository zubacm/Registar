using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class OccupancyBasedPricing
    {
        public int Id { get; set; }
        public int MinOccupancy { get; set; }
        public int MaxOccupancy { get; set; }

        public ICollection<OccupancyBasedPrices> Prices { get; set; }

    }
}
