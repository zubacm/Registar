﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TuristRegistar.Data.Models
{
    public class Objects
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Double Lat { get; set; }
        public Double Lng { get; set; }
        public String Address { get; set; }
        [StringLength(256)]
        public String EmailContact { get; set; }
        public String PhoneNumberContact { get; set; }
        public String WebContact { get; set; }
        //public int NumberOfRooms { get; set; }//maybe delete this is attribute
        public float Surface { get; set; }
        public int MinOccupancy { get; set; }
        public int MaxOccupancy { get; set; }
        public String Description { get; set; }
        public int MinDaysOffer { get; set; }
        public int MaxDaysOffer { get; set; }
        public bool StandardPricingM { get; set; }

        public ICollection<ObjectAttributes> ObjectAttributes { get; set; }
        //public ICollection<CountableObjectAttributes> CountableObjectAttributes { get; set; }
        public ICollection<CntObjAttributesCount> CntObjAttributesCount { get; set; }
        public ICollection<RatingsAndReviews> RatingsAndReviews { get; set; }
        public ICollection<SpecialOffersPrices> SpecialOffers { get; set; }
        public ICollection<AvailablePeriods> AvailablePeriods { get; set; }//This will become unavailable periods
        public ICollection<ObjectImages> ObjectImages { get; set; }
        public int ObejectTypeId { get; set; }
        public ObjectTypes ObjectType { get; set; }
        public int CreatorId { get; set; }
        public Users Creator { get; set; }
        public int StandardPricingModelId { get; set; }
        public StandardPricingModel StandardPricingModel { get; set; }
        public int OccupancyBasedPricingId { get; set; }
        public OccupancyBasedPricing OccupancyBasedPricing { get; set; }

        public int CountryId { get; set; }
        public Countries Country { get; set; }
        public int CityId { get; set; }
        public Cities City { get; set; }
    }
}
