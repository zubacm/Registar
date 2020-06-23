using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class AdministrationModel
    {
        public List<UserAdministrationModel> UsersList { get; set; }

        public List<SelectListItem> Roles { get; set; }
        public String SelectedRole { get; set; }

        public int CurrPage { get; set; }
        public Pager Pager { get; set; }
        public String SearchString { get; set; }
    }
}
