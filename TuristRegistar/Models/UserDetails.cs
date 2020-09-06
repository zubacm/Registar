using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class UserDetails
    {
        public String IdentUserId { get; set; }

        //Primary key
        public string KorisnikId { get; set; }
        //Foreign key
        //public string UserId { get; set; }
        public string Username { get; set; }

        public bool LegalPerson { get; set; }
        [Display(Name = "Ime")]
        public String Name { get; set; }
        [Display(Name = "Prezime")]
        public String LastName { get; set; }
        [Display(Name = "Kontakt adresa")]
        public String ContactAddress { get; set; }
        [Display(Name = "Telefon")]
        public String Phone { get; set; }

        [Display(Name = "Email adresa")]
        [EmailAddress]
        public String Email { get; set; }

        public bool AdminAction { get; set; }
    }
}
