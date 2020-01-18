using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class Search
    {
        public String SearchString { get; set; }

        [Display(Name = "Dolazak")]
        public DateTime CheckIn { get; set; }

        [Display(Name = "Odlazak")]
        public DateTime CheckOut { get; set; }

        [Display(Name = "Broj osoba")]
        [Range(1, 30, ErrorMessage = "Broj osoba mora biti u opsegu od 1 do 30.")]
        public int Occupancy { get; set; }

        [Display(Name = "Ocjena iznad")]
        [Range(1, 10, ErrorMessage = "Ocjena mora biti u opsegu od 1 do 10.")]
        public int RatingAbove { get; set; }

        [Display(Name = "Cijena ispod")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Unesite ispravan iznos.")]
        public int PriceBelow { get; set; }

        [Display(Name = "Tip objekta")]
        public List<ObjectTypeModel> ObjectTypes { get; set; }
        [Display(Name = "Karakteristike")]
        public List<ObjectAttributesModel> ObjectAttributes { get; set; }
    }
}
