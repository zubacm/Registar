using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class OccupancyBasedPrices
    {
        public int Id { get; set; }
        public int Occupancy { get; set; }
        public Decimal PricePerNight { get; set; }

        public int OccunapncyBasedPricingId { get; set; }
        public OccupancyBasedPricing OccunapncyBasedPricing { get; set; }
    }
}
