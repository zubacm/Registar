using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristRegistar.Data.Models;

namespace TuristRegistar.Models
{
    public class Review
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public int Rating { get; set; }
        public string IdentUserId { get; set; }
        public IdentityUser IdentUser { get; set; }
        public string UserId { get; set; }
        public Users User { get; set; }
    }
}
