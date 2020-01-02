using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class Users
    {
        [StringLength(450)]
        public String Id { get; set; }
        [StringLength(256)]
        public String UserName { get; set; }
        [StringLength(256)]
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String ContactAddress { get; set; }
        [StringLength(256)]
        public String Name { get; set; }
        [StringLength(256)]
        public String LastName { get; set; }
        public bool LegalPerson { get; set; }

        public String IdentUserId { get; set; }
        public IdentityUser IdentUser { get; set; }

        public ICollection<Objects> BookmarkedObjects { get; set; }
    }
}
