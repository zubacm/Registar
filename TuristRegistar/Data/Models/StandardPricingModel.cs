using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class StandardPricingModel
    {
        public int Id { get; set; }
        public float StandardPricePerNight { get; set; }
        public int StandardOccupancy { get; set; }
        public int MinOccupancy { get; set; }
        public int MaxOccupancy { get; set; }
        public float OffsetPercentage {get; set;}
    }
}
