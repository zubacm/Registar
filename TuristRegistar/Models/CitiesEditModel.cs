using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class CitiesEditModel
    {
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<Cities> CitiesForCountry { get; set; }
        public int SelectedCountry { get; set; }
        public int SelectedCountryAddModal { get; set; }
        public int SelectedCountryEditModal { get; set; }
        public String NewCity { get; set; }
        public int EditCityId { get; set; }
        public String EditCity { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Geografska širina")]
        public Double LatAddModal { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Geografska dužina")]
        public Double LngAddModal { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Geografska širina")]
        public Double LatEditModal { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Geografska dužina")]
        public Double LngEditModal { get; set; }

        public String SubmitButton { get; set; }
    }
}
