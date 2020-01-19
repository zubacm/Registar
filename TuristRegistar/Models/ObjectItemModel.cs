using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Models
{
    public class ObjectItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
        public string Location { get; set; }
        public Double Lat { get; set; }
        public Double Lng { get; set; }
        public string ImgSrc { get; set; }
        public string Description { get; set; }
        public int NumberOfRatings { get; set; }
        public Decimal Rating { get; set; }
        public String Type { get; set; }

        public String EmailContact { get; set; }
        public String PhoneNumberContact { get; set; }
        public String WebContact { get; set; }
    }
}
