using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class KorisnikViewModel
    {
        [Required(ErrorMessage = "Polje Korisničko ime je obavezno.")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Polje lozinka je obavezno.")]
        [StringLength(100, ErrorMessage = "{0} mora biti najmanje {2} i najviše {1} karaktera duga.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
        [StringLength(100, ErrorMessage = "{0} mora biti najmanje {2} i najviše {1} karaktera duga.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public String ConfirmPassword { get; set; }

        public bool LegalPerson { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String ContactAddress { get; set; }
        public String Phone { get; set; }

        [EmailAddress]
        public String Email { get; set; }
    }
}
