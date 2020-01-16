using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TuristRegistar.Data;
using TuristRegistar.Data.Models;

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
            var obj =  _context.Objects
                .Include(o => o.Country)
                .Include(o => o.City)
                .Include(o => o.ObjectType)
                .Include(o => o.ObjectHasAttributes)
                .Include(o => o.CntObjAttributesCount)
                .Include(o => o.SpecialOffers)
                .Include(o => o.UnavailablePeriods)
                .Include(o => o.ObjectImages)
                .Include(o => o.OccupancyBasedPricing).Include(o => o.OccupancyBasedPricing.Prices)
                .Include(o => o.StandardPricingModel)
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
            _context.AvailablePeriods.Remove(period);//nekad ovo ime promijenit
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
                    //f49a41b74f3e1f052200 => this is my api key
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
                    //return new KeyValuePair<string, string> ( "Error", "Error calling API. Please do manual lookup." );
                    //return "Error calling API. Please do manual lookup.";
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

        public void AddOccupancyBasedModel(OccupancyBasedPricing occupancybp, int objectid)
        {
            _context.OccupancyBasedPricings.Add(occupancybp);
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyBasedPricingId = occupancybp.Id;
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyPricing = true;
            _context.SaveChanges();
        }

        public void RemobeOccupancyBasedPricesRange(int occupancyId)
        {
            _context.RemoveRange(_context.OccupancyBasedPrices.Where(obp => obp.OccunapncyBasedPricingId == occupancyId));
            _context.SaveChanges();
        }

        //ako ovo bude radilo ++++
        public void NewOccupancyBasedPricing(OccupancyBasedPricing occupanybp, int objectid)
        {
            var oldId = occupanybp.Id;
            _context.Objects.FirstOrDefault(o => o.OccupancyBasedPricingId == oldId).OccupancyBasedPricingId = null;
            _context.Remove(occupanybp);
            _context.SaveChanges();
            occupanybp.Id = 0;
            _context.Add(occupanybp);
            //_context.SaveChanges();
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
        public void AddStandardModel(StandardPricingModel standardmodel, int objectid)
        {
            _context.StandardPricingModels.Add(standardmodel);
            _context.Objects.FirstOrDefault(o => o.Id == objectid).StandardPricingModelId = standardmodel.Id;
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyBasedPricingId = null;
            _context.Objects.FirstOrDefault(o => o.Id == objectid).OccupancyPricing = false;
            _context.SaveChanges();
        }
        public void EditStandardModel(StandardPricingModel standardmodel)
        {
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).MinDaysOffer = standardmodel.MinDaysOffer;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).MaxDaysOffer = standardmodel.MaxDaysOffer;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).MinOccupancy = standardmodel.MinOccupancy;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).MaxOccupancy = standardmodel.MaxOccupancy;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).StandardOccupancy = standardmodel.StandardOccupancy;
            _context.StandardPricingModels.FirstOrDefault(s => s.Id == standardmodel.Id).StandardPricePerNight = standardmodel.StandardPricePerNight;
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
    }
}
