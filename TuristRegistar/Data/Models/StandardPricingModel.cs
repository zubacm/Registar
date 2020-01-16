using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class StandardPricingModel
    {
        public int Id { get; set; }
        [Display(Name = "Standardna cijena noćenja")]
        public Nullable<Decimal> StandardPricePerNight { get; set; }
        [Display(Name = "Standardan broj posjetilaca")]
        public Nullable<int> StandardOccupancy { get; set; }
        [Display(Name = "Mininmalan broj posjetilaca")]
        public Nullable<int> MinOccupancy { get; set; }
        [Display(Name = "Maksimalan broj posjetilaca")]
        public Nullable<int> MaxOccupancy { get; set; }
        [Display(Name = "Postotak ofseta po osobi")]
        public Nullable<Decimal> OffsetPercentage {get; set;}

        [Display(Name = "Mininmalan broj dana boravka")]
        public Nullable<int> MinDaysOffer { get; set; }
        [Display(Name = "Maksimalan broj dana boravka")]
        public Nullable<int> MaxDaysOffer { get; set; }

        public Objects Objects { get; set; }
    }
}
