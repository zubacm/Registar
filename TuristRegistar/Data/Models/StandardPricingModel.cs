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
        public float StandardPricePerNight { get; set; }
        [Display(Name = "Standardan broj posjetilaca")]
        public int StandardOccupancy { get; set; }
        [Display(Name = "Mininmalan broj posjetilaca")]
        public int MinOccupancy { get; set; }
        [Display(Name = "Maksimalan broj posjetilaca")]
        public int MaxOccupancy { get; set; }
        [Display(Name = "Postotak ofseta po osobi")]
        public float OffsetPercentage {get; set;}

        [Display(Name = "Mininmalan broj dana boravka")]
        public int MinDaysOffer { get; set; }
        [Display(Name = "Maksimalan broj dana boravka")]
        public int MaxDaysOffer { get; set; }
    }
}
