using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class ObjectDetailsViewModel
    {
        public int Id { get; set; }

        public string IdentUserId { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Naziv")]
        public String Name { get; set; }
        [Display(Name = "Opis")]
        public String Description { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Geografska širina")]
        public Double Lat { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Geografska dužina")]
        public Double Lng { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Adresa")]
        public String Address { get; set; }
        [Display(Name = "Email kontakt")]
        public String EmailContact { get; set; }
        [Display(Name = "Broj telefona")]
        public String PhoneNumberContact { get; set; }
        [Display(Name = "Web kontakt")]
        public String WebContact { get; set; }
        [Display(Name = "Površina")]
        public Nullable<float> Surface { get; set; }
        [Display(Name = "Lokacija")]
        public String Location { get; set; }

        public int NumberOfRatings { get; set; }
        public Decimal Rating { get; set; }

        public bool OccupancyPricing { get; set; }
        public StandardPricingModel StandardPricingModel { get; set; }
        public OccupancyBasedPricing OccupancyBasedPricing { get; set; }

        public List<ObjectHasAttributes> Offers { get; set; }
        public List<CntObjAttributesCount> CntOffers { get; set; }
        public List<SpecialOffersPrices> SpecialOffers { get; set; }

        public List<ObjectImages> ImgsSrc { get; set; }

        [Display(Name = "Država")]
        public String Country { get; set; }
        [Display(Name = "Grad")]
        public String City { get; set; }
        [Display(Name = "Tip objekta")]
        public String ObjectType { get; set; }

        public int Occupancy { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public Decimal Price { get; set; }
    }
}
