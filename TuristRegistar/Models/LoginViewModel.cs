using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        [StringLength(100, ErrorMessage = "{0} mora biti najmanje {2} i najviše {1} karaktera duga.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}
