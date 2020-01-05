using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class SpecialOffersPrices
    {
        [Key]
        public int SpecialOfferId { get; set; }
        public ObjectAttributes SpecialOffer { get; set; }
        [Key]
        public int ObjectId { get; set; }
        public Objects Object { get; set; }

        public float Price { get; set; }
    }
}
