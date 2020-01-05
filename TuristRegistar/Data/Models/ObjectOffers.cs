using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class ObjectOffers
    {
        public int Id { get; set; }
        public int SpecialOfferId { get; set; }
        public SpecialOffersPrices SpecialOffersPrices { get; set; }
        public bool IncludedInOriginalPrice { get; set; }
        public float Price { get; set; }
    }
}
