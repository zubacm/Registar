﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class Bookmark
    {
        [Key]
        public int ObjectId { get; set; }
        public Objects Object { get; set; }
        [Key]
        public String UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
