using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;
using TuristRegistar.Models;

namespace TuristRegistar.Services
{
    public class TouristObjectService : ITouristObject
    {
        private readonly ApplicationDbContext _context;

        public TouristObjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Countries> GetCountries()
        {
            return _context.Counries.ToList();
        }

        public IEnumerable<Cities> GetCitiesFromCountry(int countryId)
        {
            return _context.Counries.Include(c => c.Cities)
                .FirstOrDefault(c => c.Id == countryId).Cities.ToList();
        }

        public IEnumerable<ObjectTypes> GetObjectTypes()
        {
            return _context.ObjectTypes;
        }


        //parsing string ex. [1,2,9,3]
        public List<int> ParseStringToIDsList(string text)
        {
            string[] input = text.Trim('[', ']').Split(',');

            int i = 0;
            return input.Where(s => int.TryParse(s, out i)) 
                             .Select(s => i)
                             .ToList();
        }

        //parsing string ex [1:2,3:12,8:2]
        public List<KeyValuePair<int, int>> ParseStringToKeyValueList(string text) {
            var listKeyValue = new List<KeyValuePair<int, int>>();
            string[] input = text.Trim('[', ']').Split(',');

            foreach (var item in input)
            {
                var pair = item.ToString().Split(':');
                listKeyValue.Add(new KeyValuePair<int, int>(Convert.ToInt32(pair[0]), Convert.ToInt32(pair[1]) ));
            }
            return listKeyValue; 
        }

        public List<KeyValuePair<int, Decimal>> ParseStringToKeyValue(string text)
        {
            var listKeyValue = new List<KeyValuePair<int, Decimal>>();
            string[] input = text.Trim('[', ']').Split(',');

            foreach (var item in input)
            {
                var pair = item.ToString().Split(':');
                listKeyValue.Add(new KeyValuePair<int, Decimal>(Convert.ToInt32(pair[0]), Convert.ToDecimal(pair[1])));
            }
            return listKeyValue;
        }

        public List<KeyValuePair<DateTime, DateTime>> ParseDates(string text)
        {
            var listKeyValue = new List<KeyValuePair<DateTime, DateTime>>();
            string[] input = text.Trim('[', ']').Split(',');

            foreach (var item in input)
            {
                var pair = item.ToString().Split(':');
                listKeyValue.Add(new KeyValuePair<DateTime, DateTime>(DateTime.Parse(pair[0]), DateTime.Parse(pair[1])));
            }
            return listKeyValue;
        }

        public DateTime ParseDate(string text)
        {
            var listKeyValue = new List<KeyValuePair<DateTime, DateTime>>();
            string[] input = text.Trim('[', ']').Split(',');

            return DateTime.Parse(input[0]);
            
        }

        public IEnumerable<ObjectAttributes> GetAllObjectAttributes()
        {
            return _context.ObjectAttributes.ToList();
        }

        public IEnumerable<ObjectAttributes> GetObjectAttributes(List<int> excludedAttributesId)
        {
            return _context.ObjectAttributes.Where(oa => !excludedAttributesId.Contains(oa.Id))
                .ToList();
        }

        public IEnumerable<CountableObjectAttributes> GetAllCountableObjectAttributes()
        {
            return _context.CountableObjectAttributes.ToList();
        }

        public IEnumerable<CountableObjectAttributes> GetCountableObjectAttributes(List<int> excludedAttributesId)
        {
            return _context.CountableObjectAttributes.Where(item => !excludedAttributesId.Contains(item.Id))
               .ToList();
        }


        public CountableObjectAttributes GetCountableObjectAttribute(int id)
        {
            return _context.CountableObjectAttributes.FirstOrDefault(coa => coa.Id == id);
        }

        public ObjectAttributes GetObjectAttribute(int id)
        {
            return _context.ObjectAttributes.FirstOrDefault(oa => oa.Id == id);
        }


        public async Task AddObject(Objects myobject, string currency)
        {

            myobject = await ExchangeCurrencyAsync(myobject, currency, "BAM");

            _context.Objects.Add(myobject);
            _context.SaveChanges();
        }

        public async Task<Objects> GetObject(int id, string currency)
        {
            var obj = _context.Objects
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes).ThenInclude(o => o.Attribute)
                .Include(o => o.CntObjAttributesCount).ThenInclude(o => o.CountableObjAttr)
                .Include(o => o.SpecialOffers).ThenInclude(o => o.SpecialOffer)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.ObjectImages)
                .Include(o => o.OccupancyBasedPricing).Include(o => o.OccupancyBasedPricing.Prices)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.Creator)
                .Include(o => o.IdentUser)
                .FirstOrDefault(o => o.Id == id);
            obj = await ExchangeCurrencyAsync(obj, "BAM", currency);


            return obj;

        }

        public int AddImage(ObjectImages image, int objectid)
        {
            var obj = _context.Objects.FirstOrDefault(ob => ob.Id == objectid);
            image.Objects = obj;
            _context.ObjectImages.Add(image);
            _context.SaveChanges();
            return image.Id;
        }

        public void DeleteImage(int id)
        {
            var img = new ObjectImages() { Id = id};
            _context.ObjectImages.Remove(img);
            _context.SaveChanges();
        }

        public int AddPeriod(UnavailablePeriods period, int objectid)
        {
            var obj = _context.Objects.FirstOrDefault(ob => ob.Id == objectid);
            period.Objects = obj;
            _context.AvailablePeriods.Add(period);
            _context.SaveChanges();
            return period.Id;
        }

        public void DeletePeriod(int id)
        {
            var period = new UnavailablePeriods() { Id = id };
            _context.AvailablePeriods.Remove(period);
            _context.SaveChanges();
        }


        private async Task<Objects> ExchangeCurrencyAsync(Objects myobject, string from, string to)
        {
            var exchangerate = await GetExchangeRate(from, to);
            foreach (var item in myobject.SpecialOffers)
            {
                
                item.Price = Math.Round(exchangerate * item.Price, 2);
            }
            if (myobject.OccupancyPricing)
            {
                foreach (var item in myobject.OccupancyBasedPricing.Prices)
                {
                    item.PricePerNight = Math.Round(exchangerate*item.PricePerNight, 2);
                }
            }
            else
            {
                myobject.StandardPricingModel.StandardPricePerNight = Math.Round((Decimal)(exchangerate * myobject.StandardPricingModel.StandardPricePerNight), 2);
            }

            return myobject;
        }


        public async Task<Decimal> GetExchangeRate(string from, string to)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://free.currencyconverterapi.com");
                    //f49a41b74f3e1f052200 => api key
                    var response = await client.GetAsync($"/api/v6/convert?q={from}_{to}&compact=y&apiKey=f49a41b74f3e1f052200");


                    var stringResult = await response.Content.ReadAsStringAsync();
                    var dictResult = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(stringResult);

                    //key
                    var mykey = dictResult.ElementAt(0).Key;
                    var myval = dictResult.ElementAt(0).Value.ElementAt(0).Value;
                    //value
                    return Convert.ToDecimal(dictResult.ElementAt(0).Value.ElementAt(0).Value);
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    return 0;
                }
            }
        }

        public void DeleteStandardModel(int objectid)
        {
            var obj = _context.Objects.FirstOrDefault(o => o.Id == objectid);
            var standardmodel = _context.StandardPricingModels.FirstOrDefault(sm => sm.Objects.Id == objectid);
            _context.StandardPricingModels.Remove(standardmodel);
            _context.SaveChanges();
        }

        public async Task AddOccupancyBasedModel(OccupancyBasedPricing occupancybp, int objectid, string currency)
        {
            var curr_correlation = await GetExchangeRate(currency, "BAM");

            foreach (var item in occupancybp.Prices)
            {
                item.PricePerNight *= curr_correlation;
            }
            _context.OccupancyBasedPricings.Add(occupancybp);
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyBasedPricingId = occupancybp.Id;
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyPricing = true;
            _context.Objects.FirstOrDefault(o => o.Id == objectid).StandardPricingModelId = null;
            _context.SaveChanges();
        }

        public void RemobeOccupancyBasedPricesRange(int occupancyId)
        {
            _context.RemoveRange(_context.OccupancyBasedPrices.Where(obp => obp.OccunapncyBasedPricingId == occupancyId));
            _context.SaveChanges();
        }

        public async Task NewOccupancyBasedPricing(OccupancyBasedPricing occupanybp, int objectid, string currency)
        {
            var curr_correlation = await GetExchangeRate(currency, "BAM");

            var oldId = occupanybp.Id;
            _context.Objects.FirstOrDefault(o => o.OccupancyBasedPricingId == oldId).OccupancyBasedPricingId = null;
            _context.Remove(occupanybp);
            _context.SaveChanges();
            occupanybp.Id = 0;
            foreach (var item in occupanybp.Prices)
            {
                item.PricePerNight *= curr_correlation;
            }
            _context.Add(occupanybp);
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyBasedPricingId = occupanybp.Id;
            _context.SaveChanges();
        }


        public void DeleteOccupancyBasedModel(int objectid)
        {
            var obj = _context.Objects.FirstOrDefault(o => o.Id == objectid);
            var occupancyBModel = _context.OccupancyBasedPricings.FirstOrDefault(ob => ob.Objects.Id == objectid);
            _context.OccupancyBasedPricings.Remove(occupancyBModel);
            _context.SaveChanges();
        }
        public async Task AddStandardModel(StandardPricingModel standardmodel, int objectid, string currency)
        {
            var curr_correlation = await GetExchangeRate(currency, "BAM");

            standardmodel.StandardPricePerNight *= curr_correlation;
            _context.StandardPricingModels.Add(standardmodel);
            _context.Objects.FirstOrDefault(o => o.Id == objectid).StandardPricingModelId = standardmodel.Id;
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyBasedPricingId = null;
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyPricing = false;
            _context.SaveChanges();
        }
        public async Task EditStandardModel(StandardPricingModel standardmodel, string currency)
        {
            var curr_correlation = await GetExchangeRate(currency, "BAM");

            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).MinDaysOffer = standardmodel.MinDaysOffer;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).MaxDaysOffer = standardmodel.MaxDaysOffer;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).MinOccupancy = standardmodel.MinOccupancy;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).MaxOccupancy = standardmodel.MaxOccupancy;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).StandardOccupancy = standardmodel.StandardOccupancy;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).StandardPricePerNight = standardmodel.StandardPricePerNight*curr_correlation;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).OffsetPercentage = standardmodel.OffsetPercentage;
            _context.SaveChanges();
        }

        public void DeleteObjectHasAttributes(int objectid, int attributeid)
        {
            var objHA = _context.ObjectHasAttributes.FirstOrDefault(oha => oha.ObjectId == objectid && oha.AttributeId == attributeid);
            _context.ObjectHasAttributes.Remove(objHA);
            _context.SaveChanges();
        }

        public void AddObjectHasAttribute(ObjectHasAttributes objHasAttribute)
        {
            _context.ObjectHasAttributes.Add(objHasAttribute);
            _context.SaveChanges();
        }

        public void AddCntAttributeCount(CntObjAttributesCount cntAttrCount)
        {
            _context.CntObjAttributesCount.Add(cntAttrCount);
            _context.SaveChanges();
        }
        public void DeleteCntAttributeCount(int objectid, int cntattributeid)
        {
            var cntAttrCount = _context.CntObjAttributesCount.FirstOrDefault(c => c.ObjectId == objectid && c.CountableObjAttrId == cntattributeid);
            _context.CntObjAttributesCount.Remove(cntAttrCount);
            _context.SaveChanges();
        }

        public void AddSpecialOffer(SpecialOffersPrices specialoffer)
        {
            _context.SpecialOffersPrices.Add(specialoffer);
            _context.SaveChanges();
        }
        public void DeleteSpecialOffer(int objectid, int attributeid)
        {
            var specialoffer = _context.SpecialOffersPrices.FirstOrDefault(s => s.ObjectId == objectid && s.SpecialOfferId == attributeid);
            _context.SpecialOffersPrices.Remove(specialoffer);
            _context.SaveChanges();
        }

        public void EditObjectBasic(Objects myobject)
        {
            var obj = _context.Objects.FirstOrDefault(o => o.Id == myobject.Id);
            obj.Name = myobject.Name;
            obj.Lat = myobject.Lat;
            obj.Lng = myobject.Lng;
            obj.Address = myobject.Address;
            obj.EmailContact = myobject.EmailContact;
            obj.PhoneNumberContact = myobject.PhoneNumberContact;
            obj.WebContact = myobject.WebContact;
            obj.Description = myobject.Description;
            obj.Surface = myobject.Surface;
            obj.CountryId = myobject.CountryId;
            obj.CityId = myobject.CityId;
            obj.ObjectTypeId = myobject.ObjectTypeId;
            obj.FullAddress = myobject.FullAddress;

            _context.Objects.Attach(obj);
            _context.SaveChanges();
        }

        public IEnumerable<Objects> GetAllObjects()
        {
            return _context.Objects
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                ;
        }

        public IEnumerable<Objects> GetObjects(int pagenumber, int pagesize)
        {
            return _context.Objects
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
        }

        public async Task<IEnumerable<Objects>> GetFilteredObjects(int pagenumber, int pagesize, Search search, string currency)
        {
            var myobjects = GetCheckedRatingAndSearch(search);

            if (search.Occupancy != 0 && search.PriceBelow != 0 && search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                var priceBelowBAM = (await GetExchangeRate(currency, "BAM")) * search.PriceBelow;
                int numberOfDays = (search.CheckOut - search.CheckIn).Days;

                myobjects = myobjects
                    .Where(ob => ((ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer)))
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()))
                    .Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy && GetPriceForOccupancyModel(ob.Id, search.Occupancy)*numberOfDays <= priceBelowBAM)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy && GetPriceForStandardModel(ob.Id,search.Occupancy)*numberOfDays <= priceBelowBAM));
            }
            else if (search.Occupancy != 0 && search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                myobjects = myobjects
                    .Where(ob => ((ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer))) 
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()))
                    .Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy));
            }
            else if (search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                myobjects = myobjects
                    .Where(ob => (ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer))
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()));
            }
            else if (search.Occupancy != 0)
            {
                myobjects = myobjects.Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy));
            }

            return myobjects.Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
        }


        public async Task<IEnumerable<Objects>> GetAllFilteredObjects(Search search, string currency)
        {
            var myobjects = GetCheckedRatingAndSearch(search);

            if (search.Occupancy != 0 && search.PriceBelow != 0 && search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                var priceBelowBAM = (await GetExchangeRate(currency, "BAM")) * search.PriceBelow;
                int numberOfDays = (search.CheckOut - search.CheckIn).Days;

                myobjects = myobjects
                    .Where(ob => ((ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer)))
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()))
                    .Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy && GetPriceForOccupancyModel(ob.Id, search.Occupancy) * numberOfDays <= priceBelowBAM)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy && GetPriceForStandardModel(ob.Id, search.Occupancy) * numberOfDays <= priceBelowBAM));
            }
            else if (search.Occupancy != 0 && search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                myobjects = myobjects
                    .Where(ob => ((ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer)))
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()))
                    .Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy));
            }
            else if (search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                myobjects = myobjects
                    .Where(ob => (ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer))
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()));
            }
            else if (search.Occupancy != 0)
            {
                myobjects = myobjects.Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy));
            }

            return myobjects.ToList();
        }





        public bool checkNumberOfStayDays(DateTime checkIn, DateTime checkOut, int? minDays = 0, int? maxDays = 0)
        {
            int numberOfDays = (checkOut - checkIn).Days;
            if (minDays > 0 && numberOfDays < minDays)
                return false;
            if (maxDays > 0 && numberOfDays > maxDays)
                return false;

            return true;

        }
        public bool CheckCheckInAndOut(DateTime checkIn, DateTime checkOut, List<UnavailablePeriods> unavailablePeriods)
        {
            foreach (var period in unavailablePeriods)
            {
                if ((DateTime.Compare(period.From, checkIn) <= 0 && DateTime.Compare(period.To, checkIn) >= 0) || (DateTime.Compare(period.To, checkOut) >= 0 && DateTime.Compare(period.From, checkOut) <= 0) )
                    return false;
            }
            return true;
        }
        private StandardPricingModel GetStandardPricingModel(int objectId)
        {
            return _context.StandardPricingModels.FirstOrDefault(spm => spm.Objects.Id == objectId);
        }
        private OccupancyBasedPricing GetOccupancyBasedPricing(int objectId)
        {
            return _context.OccupancyBasedPricings.FirstOrDefault(obp => obp.Objects.Id == objectId);
        }
        public StandardPricingModel GetStandardPricingModelWithId(int id)
        {
            return _context.StandardPricingModels.FirstOrDefault(spm => spm.Id == id);
        }
        public OccupancyBasedPricing GetOccupancyBasedPricingWithId(int id)
        {
            return _context.OccupancyBasedPricings
                .Include(obp => obp.Prices)
                .FirstOrDefault(obp => obp.Id == id);
        }

        private Decimal GetPriceForStandardModel(int objectId, int occupancy)
        {
            var standardPM = GetStandardPricingModel(objectId);
            var occupancyOffset = occupancy - standardPM.StandardOccupancy;
            return (Decimal)standardPM.StandardPricePerNight + (Decimal)(occupancyOffset * standardPM.StandardPricePerNight * (standardPM.OffsetPercentage / 100));
        }
        
        private Decimal GetPriceForOccupancyModel(int objectId, int occupancy)
        {
            var occupancyPM = _context.OccupancyBasedPricings.Include(opm => opm.Prices).FirstOrDefault(obp => obp.Objects.Id == objectId);
            return occupancyPM.Prices.FirstOrDefault(pr => pr.Occupancy == occupancy).PricePerNight;            
        }
        private IEnumerable<Objects> GetCheckedRatingAndSearch(Search search)
        {
            var checkedTypes = search.ObjectTypes.Where(ot => ot.Selected == true).Select(ot => ot.Id).ToList();
            var checkedAttributes = search.ObjectAttributes.Where(oa => oa.Selected == true).Select(oa => oa.Id).ToList();
            if (checkedTypes.Count > 0 && checkedAttributes.Count > 0 && search.RatingAbove != 0 && (!string.IsNullOrWhiteSpace(search.SearchString)))
            {
                return _context.Objects
                .Where(o => checkedTypes.Contains((int)o.ObjectTypeId))
                .Where(o => !checkedAttributes.Except(o.ObjectHasAttributes.Select(item => item.AttributeId)).Any())
                .Where(o => (o.RatingsAndReviews.Count > 0 ? o.RatingsAndReviews.Select(item => item.Rating).Average() >= search.RatingAbove : false))
                .Where(ob => ob.Name.ToLower().Contains(search.SearchString.ToLower()) || ob.FullAddress.ToLower().Contains(search.SearchString.ToLower()))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedTypes.Count > 0 && checkedAttributes.Count > 0 && search.RatingAbove != 0)
            {
                return _context.Objects
                .Where(o => checkedTypes.Contains((int)o.ObjectTypeId))
                .Where(o => !checkedAttributes.Except(o.ObjectHasAttributes.Select(item => item.AttributeId)).Any())
                .Where(o => (o.RatingsAndReviews.Count > 0 ? o.RatingsAndReviews.Select(item => item.Rating).Average() >= search.RatingAbove : false))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedTypes.Count > 0 && checkedAttributes.Count > 0 && (!string.IsNullOrWhiteSpace(search.SearchString)))
            {
                return _context.Objects
                .Where(o => checkedTypes.Contains((int)o.ObjectTypeId))
                .Where(o => !checkedAttributes.Except(o.ObjectHasAttributes.Select(item => item.AttributeId)).Any())
                .Where(ob => ob.Name.ToLower().Contains(search.SearchString.ToLower()) || ob.FullAddress.ToLower().Contains(search.SearchString.ToLower()))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedTypes.Count > 0 && search.RatingAbove != 0 && (!string.IsNullOrWhiteSpace(search.SearchString)))
            {
                return _context.Objects
                .Where(o => checkedTypes.Contains((int)o.ObjectTypeId))
                .Where(o => (o.RatingsAndReviews.Count > 0 ? o.RatingsAndReviews.Select(item => item.Rating).Average() >= search.RatingAbove : false))
                .Where(ob => ob.Name.ToLower().Contains(search.SearchString.ToLower()) || ob.FullAddress.ToLower().Contains(search.SearchString.ToLower()))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedAttributes.Count > 0 && search.RatingAbove != 0 && (!string.IsNullOrWhiteSpace(search.SearchString)))
            {
                return _context.Objects
                .Where(o => !checkedAttributes.Except(o.ObjectHasAttributes.Select(item => item.AttributeId)).Any())
                .Where(o => (o.RatingsAndReviews.Count > 0 ? o.RatingsAndReviews.Select(item => item.Rating).Average() >= search.RatingAbove : false))
                .Where(ob => ob.Name.ToLower().Contains(search.SearchString.ToLower()) || ob.FullAddress.ToLower().Contains(search.SearchString.ToLower()))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedTypes.Count > 0 && search.RatingAbove != 0)
            {
                return _context.Objects
                .Where(o => checkedTypes.Contains((int)o.ObjectTypeId))
                .Where(o => (o.RatingsAndReviews.Count > 0 ? o.RatingsAndReviews.Select(item => item.Rating).Average() >= search.RatingAbove : false))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedTypes.Count > 0 && (!string.IsNullOrWhiteSpace(search.SearchString)))
            {
                return _context.Objects
                .Where(o => checkedTypes.Contains((int)o.ObjectTypeId))
                .Where(ob => ob.Name.ToLower().Contains(search.SearchString.ToLower()) || ob.FullAddress.ToLower().Contains(search.SearchString.ToLower()))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedAttributes.Count > 0 && search.RatingAbove != 0)
            {
                return _context.Objects
                .Where(o => !checkedAttributes.Except(o.ObjectHasAttributes.Select(item => item.AttributeId)).Any())
                .Where(o => (o.RatingsAndReviews.Count > 0 ? o.RatingsAndReviews.Select(item => item.Rating).Average() >= search.RatingAbove : false))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedAttributes.Count > 0 && (!string.IsNullOrWhiteSpace(search.SearchString)))
            {
                return _context.Objects
                .Where(o => !checkedAttributes.Except(o.ObjectHasAttributes.Select(item => item.AttributeId)).Any())
                .Where(ob => ob.Name.ToLower().Contains(search.SearchString.ToLower()) || ob.FullAddress.ToLower().Contains(search.SearchString.ToLower()))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (search.RatingAbove != 0 && (!string.IsNullOrWhiteSpace(search.SearchString)))
            {
                return _context.Objects
                .Where(o => (o.RatingsAndReviews.Count > 0 ? o.RatingsAndReviews.Select(item => item.Rating).Average() >= search.RatingAbove : false))
                .Where(ob => ob.Name.ToLower().Contains(search.SearchString.ToLower()) || ob.FullAddress.ToLower().Contains(search.SearchString.ToLower()))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedTypes.Count > 0 && checkedAttributes.Count > 0)
            {
                return _context.Objects
                .Where(o => checkedTypes.Contains((int)o.ObjectTypeId))
                .Where(o => !checkedAttributes.Except(o.ObjectHasAttributes.Select(item => item.AttributeId)).Any())
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedTypes.Count > 0)
            {
                return _context.Objects
                .Where(o => checkedTypes.Contains((int)o.ObjectTypeId))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (checkedAttributes.Count > 0)
            {
                return _context.Objects
                .Where(o => !checkedAttributes.Except(o.ObjectHasAttributes.Select(item => item.AttributeId)).Any())
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (search.RatingAbove != 0)
            {
                return _context.Objects
                .Where(o => (o.RatingsAndReviews.Count > 0 ? o.RatingsAndReviews.Select(item => item.Rating).Average() >= search.RatingAbove : false))
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.RatingsAndReviews)
                .Include(o => o.ObjectImages)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.StandardPricingModel)
                .Include(o => o.OccupancyBasedPricing)
                .ToList();
            }
            else if (!string.IsNullOrWhiteSpace(search.SearchString))
            {
                return _context.Objects//check the city, country is missing
                    .Where(ob => ob.Name.ToLower().Contains(search.SearchString.ToLower()) || ob.FullAddress.ToLower().Contains(search.SearchString.ToLower()))
                    .Include(o => o.Country)
                    .Include(o => o.City)
                    .Include(o => o.ObjectType)
                    .Include(o => o.ObjectHasAttributes)
                    .Include(o => o.CntObjAttributesCount)
                    .Include(o => o.RatingsAndReviews)
                    .Include(o => o.ObjectImages)
                    .Include(o => o.UnavailablePeriods)
                    .Include(o => o.StandardPricingModel)
                    .Include(o => o.OccupancyBasedPricing)
                    .ToList();
            }
            return _context.Objects
            .Include(o => o.Country)
            .Include(o => o.City)
            .Include(o => o.ObjectType)
            .Include(o => o.ObjectHasAttributes)
            .Include(o => o.CntObjAttributesCount)
            .Include(o => o.RatingsAndReviews)
            .Include(o => o.ObjectImages)
            .Include(o => o.UnavailablePeriods)
            .Include(o => o.StandardPricingModel)
            .Include(o => o.OccupancyBasedPricing)
            .ToList();

        }



        public async Task<int> CountFilteredObjects(Search search, string currency)
        {
            var myobjects = GetCheckedRatingAndSearch(search);

            if (search.Occupancy != 0 && search.PriceBelow != 0 && search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                var priceBelowBAM = (await GetExchangeRate(currency, "BAM")) * search.PriceBelow;
                int numberOfDays = (search.CheckOut - search.CheckIn).Days;

                return myobjects
                    .Where(ob => ((ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer)))
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()))
                    .Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy && GetPriceForOccupancyModel(ob.Id, search.Occupancy) * numberOfDays <= priceBelowBAM)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy && GetPriceForStandardModel(ob.Id, search.Occupancy) * numberOfDays <= priceBelowBAM))
                .Count();
            }
            else if (search.Occupancy != 0 && search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                return myobjects
                    .Where(ob => ((ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer)))
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()))
                    .Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy))
                .Count(); 
            }
            else if (search.CheckIn != DateTime.MinValue && search.CheckOut != DateTime.MinValue)
            {
                return myobjects
                    .Where(ob => (ob.StandardPricingModelId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.StandardPricingModel.MinDaysOffer, ob.StandardPricingModel.MaxDaysOffer))
                    || (ob.OccupancyBasedPricingId != null && checkNumberOfStayDays(search.CheckIn, search.CheckOut, ob.OccupancyBasedPricing.MinDaysOffer, ob.OccupancyBasedPricing.MaxDaysOffer)))
                    .Where(ob => CheckCheckInAndOut(search.CheckIn, search.CheckOut, ob.UnavailablePeriods.ToList()))
                    .Count();
            }
            else if (search.Occupancy != 0)
            {
                return myobjects.Where(ob => (ob.OccupancyBasedPricingId != null && GetOccupancyBasedPricing(ob.Id).MinOccupancy <= search.Occupancy && GetOccupancyBasedPricing(ob.Id).MaxOccupancy >= search.Occupancy)
                || (ob.StandardPricingModelId != null && GetStandardPricingModel(ob.Id).MinOccupancy <= search.Occupancy && GetStandardPricingModel(ob.Id).MaxOccupancy >= search.Occupancy))
                .Count();
            }


            return myobjects.Count();
        }


        public int CountObjects()
        {
            return _context.Objects.Count();
        }

        public int GetNumberOfRatings(int objectId)
        {
            return _context.RatingsAndReviews.Where(r => r.ObjectId == objectId).Count();
        }
        public Decimal GetAvarageRating(int objectId)
        {
            return GetNumberOfRatings(objectId) > 0 ?
                (Decimal)_context.RatingsAndReviews.Where(r => r.ObjectId == objectId).Average(ob => ob.Rating)
                : 0;
        }

        //Expecting 1,2,34,32 ---- not used i think
        public List<int> ParseComaSeparatedStringToIntList(string text)
        {
            string[] input = text.Split(',');
            int i = 0;
            return input.Where(s => int.TryParse(s, out i))
                             .Select(s => i)
                             .ToList();
        }


        public String GetCityName(int cityId)
        {
            return _context.Cities.FirstOrDefault(c => c.Id == cityId).Name;
        }
        public String GetCoutnryName(int countryId)
        {
            return _context.Counries.FirstOrDefault(c => c.Id == countryId).Name;
        }

        public async Task<decimal> GetPriceForStandardModel(int occupancy, string currency, DateTime checkIn, DateTime checkOut, StandardPricingModel standardModel)
        {
            var curr_correlation = await GetExchangeRate("BAM", currency);
            var daysNum = (checkOut - checkIn).Days;
            var occupancyOffset = occupancy - standardModel.StandardOccupancy;
            return Math.Round(daysNum*curr_correlation *occupancy*((Decimal)standardModel.StandardPricePerNight + (Decimal)(occupancyOffset * standardModel.StandardPricePerNight * (standardModel.OffsetPercentage / 100))), 2);
 }
        public async Task<decimal> GetPriceForOccupancyBasedModel(int occupancy, string currency, DateTime checkIn, DateTime checkOut, OccupancyBasedPricing occupancyModel)
        {
            var curr_correlation = await GetExchangeRate("BAM", currency);
            var daysNum = (checkOut - checkIn).Days;
            return Math.Round(occupancyModel.Prices.FirstOrDefault(pr => pr.Occupancy == occupancy).PricePerNight * daysNum * curr_correlation, 2);
        }

        public int CountRatingsAndReviews(int objectId)
        {
            return _context.RatingsAndReviews.Where(rr => rr.ObjectId == objectId).Count();
        }

        //use this
        public IEnumerable<RatingsAndReviews> GetRatingsAndReviews(int pagenumber, int pagesize, int objectId)
        {
            return _context.RatingsAndReviews.Where(rr => rr.ObjectId == objectId)
                .Include(u => u.User)
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
        }

        public void AddRatingAndReview(RatingsAndReviews ratingAndReview)
        {
            _context.RatingsAndReviews.Add(ratingAndReview);
            _context.SaveChanges();
        }


        public Coordinates GetCityCoordinates(String search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                var city = _context.Cities.FirstOrDefault(c => c.Name.ToLower().Equals(search.ToLower()));
                if (city != null)
                    return new Coordinates() { Lat = city.Lat, Lng = city.Lng, };
            }
            return null;
        }
        public Coordinates GetCountryCoordinates(String search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                var country = _context.Counries.FirstOrDefault(c => c.Name.ToLower().Equals(search.ToLower()));
                if (country != null)
                    return new Coordinates() { Lat = country.Lat, Lng = country.Lng, };
            }
            return null;
        }

        public void DeleteReview(int id)
        {
            var rating = _context.RatingsAndReviews.Where(r => r.Id == id).FirstOrDefault();
            _context.RatingsAndReviews.Remove(rating);
            _context.SaveChanges();
        }

        public void DeleteObjectAndRelatedColumns(int objectId)
        {
            var images = _context.ObjectImages.Where(i => i.Objects.Id == objectId);
            var periods = _context.AvailablePeriods.Where(a => a.Objects.Id == objectId);
            var bookmarks = _context.Bookmark.Where(b => b.ObjectId == objectId);
            var objHasAttr = _context.ObjectHasAttributes.Where(oha => oha.ObjectId == objectId);
            var spcOffersPrices = _context.SpecialOffersPrices.Where(so => so.ObjectId == objectId);

            var obj = _context.Objects.Where(o => o.Id == objectId).FirstOrDefault();
            if (obj.StandardPricingModelId != null)
            {
                var standPM = _context.StandardPricingModels.Where(spm => spm.Id == obj.StandardPricingModelId).FirstOrDefault();//ovo nije range nego jedno
                _context.StandardPricingModels.Remove(standPM);
            }
            else if (obj.OccupancyBasedPricingId != null)
            {
                var occupancyPrices = _context.OccupancyBasedPrices.Where(obp => obp.OccunapncyBasedPricingId == obj.OccupancyBasedPricingId);
                var occupancyBasedModel = _context.OccupancyBasedPricings.Where(obm => obm.Id == obj.OccupancyBasedPricingId).FirstOrDefault();// i ovo je jedno
                _context.OccupancyBasedPrices.RemoveRange(occupancyPrices);
                _context.OccupancyBasedPricings.Remove(occupancyBasedModel);
            }

            _context.ObjectImages.RemoveRange(images);
            _context.AvailablePeriods.RemoveRange(periods);
            _context.Bookmark.RemoveRange(bookmarks);
            _context.ObjectHasAttributes.RemoveRange(objHasAttr);
            _context.SpecialOffersPrices.RemoveRange(spcOffersPrices);
            _context.Objects.Remove(obj);
            _context.SaveChanges();
        }
    }
}
