using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class UserSettingsAdminModel
    {
        public String IdentUserId { get; set; }
        public String Username { get; set; }

        public List<SelectListItem> Roles { get; set; }
        public String SelectedRoleId { get; set; }
    }
}
