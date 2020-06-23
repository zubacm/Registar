using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class UserAdministrationModel
    {
        public string Id { get; set; }
        public string IdentUserId { get; set; }

        public bool IsBanned { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public String RoleId { get; set; }
        public String RoleName { get; set; }

        public String UserName { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String TypeOfUser { get; set; }
        public String EmailAddress { get; set; }
        public String PhoneContact { get; set; }
        public String ContactAddress { get; set; }
    }
}
