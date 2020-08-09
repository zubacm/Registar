using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class Conversations
    {
        public int Id { get; set; }

        public String IdentUser1Id { get; set; }
        public IdentityUser IdentUser1 { get; set; }

        public String IdentUser2Id { get; set; }
        public IdentityUser IdentUser2 { get; set; }
    }
}
