﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class RatingsAndReviews
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public String Review { get; set; }

        public String UserId { get; set; }
        public Users User { get; set; }
        public int ObjectId { get; set; }
        public Objects Object { get; set; }
    }
}
