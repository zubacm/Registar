using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class OccupancyBasedPricing
    {
        public int Id { get; set; }
        [Display(Name = "Minimalan broj posjetilaca")]
        public int MinOccupancy { get; set; }
        [Display(Name = "Maksimalan broj posjetilaca")]
        public int MaxOccupancy { get; set; }

        [Display(Name = "Mininmalan broj dana boravka")]
        public int MinDaysOffer { get; set; }
        [Display(Name = "Maksimalan broj dana boravka")]
        public int MaxDaysOffer { get; set; }

        public ICollection<OccupancyBasedPrices> Prices { get; set; }

    }
}
