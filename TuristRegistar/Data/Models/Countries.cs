using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class Countries
    {
        public int Id { get; set; }
        public String Name { get; set; }

        //center
        public Double Lat { get; set; }
        public Double Lng { get; set; }

        public ICollection<Cities> Cities { get; set; }
    }
}
