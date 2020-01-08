using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class CreateObjectViewModel
    {
        //[Required(ErrorMessage = "Obavezno polje")]
        [Display(Name="Naziv")]
        public String Name { get; set; }
        [Display(Name = "Opis")]
        public String Description { get; set; }
        //[Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Geografska širina")]
        public Double Lat { get; set; }
        //[Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Geografska dužina")]
        public Double Lng { get; set; }
        //[Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Adresa")]
        public String Address { get; set; }
        [Display(Name="Email kontakt")]
        public String EmailContact { get; set; }
        [Display(Name = "Broj telefona")]
        public String PhoneNumberContact { get; set; }
        [Display(Name = "Web kontakt")]
        public String WebContact { get; set; }
        [Display(Name = "Površina")]
        public float Surface { get; set; }
       

        public bool StandardPricingM { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
        public String SelectedCountry { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public String SelectedCity { get; set; }

        public IEnumerable<SelectListItem> Offers { get; set; }
        public String AddedOffers { get; set; } 
        public List<int> ListOfAddedOffersId { get; set; }

        public IEnumerable<SelectListItem> CountableOffers { get; set; }
        public String AddedCountableOffers { get; set; }
        public List<CountableOffers> ListOfAddedCntOffers { get; set; }

        public IEnumerable<SelectListItem> SpecialOffers { get; set; }
        public String AddedSpecialOffers { get; set; }
        public List<SpecialOffers> ListOfAddedSpecialOffersId { get; set; }

        [Display(Name = "Mininmalan broj dana boravka")]
        public int MinDaysOffer { get; set; }
        [Display(Name = "Maksimalan broj dana boravka")]
        public int MaxDaysOffer { get; set; }

        public bool StandardPrModel { get; set; } 
        public StandardPricingModel StandardPricingModel { get; set; }
        public OccupancyBasedPricing OccupancyBasedPricing { get; set; }
        public string OccubancBasedPrices { get; set; }

        public string UnavailablePeriodsString { get; set; }
        public List<UnavailablePeriods> UnavailablePeriods { get; set; }

        public List<IFormFile> ImgFiles { get; set; }

        public string ProductName { get; set; }
        public string Title { get; set; }
        //public string Description { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> ImageId { get; set; }
        //public HttpPostedFileWrapper ImageFile { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}


