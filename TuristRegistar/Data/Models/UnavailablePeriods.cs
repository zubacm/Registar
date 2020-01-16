using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class UnavailablePeriods
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Objects Objects { get; set; }
    }
}
