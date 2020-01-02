using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class UpdateProfileViewModel
    {
        //Primary key
        public string KorisnikId { get; set; }
        //Foreign key
        public string UserId { get; set; }
        public string Username { get; set; }

        public bool LegalPerson { get; set; }
        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        public String Name { get; set; }
        public String LastName { get; set; }
        public String ContactAddress { get; set; }
        public String Phone { get; set; }

        [EmailAddress]
        public String Email { get; set; }
    }
}
